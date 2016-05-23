import urllib
import json
from time import sleep
import sys
from twisted.internet import reactor
from autobahn.twisted.websocket import WebSocketClientFactory, WebSocketClientProtocol, connectWS
global curr_game_dict, updates_json
curr_game_dict = {"inning": -1, "home_score": -1, "away_score": -1, "balls": -1, "strikes": -1, "outs": -1, "at_bat": "-1", "photo": "-1"}
updates_json = ""
url = "http://gd2.mlb.com/components/game/mlb/year_2016/month_05/day_23/gid_2016_05_23_clemlb_chamlb_1/plays.json"
photo = "http://mlb.mlb.com/mlb/images/players/head_shot/"

class BroadcastClientProtocol(WebSocketClientProtocol):
    def onOpen(self):
        print "Socket Opened"
        self.sendMessage(str(updates_json).encode('utf-8'))
        print "Updates sent to Server"
    def onMessage(self, payload, isBinary):
        if not isBinary:
            print("Server said: {}".format(payload.decode('utf8')))
        self.factory.reactor.stop()


def send_updates(updates):
    global updates_json
    print "sending updates"
    updates_json = json.dumps(updates)
    try:
        reactor.run()
    except KeyboardInterrupt:
        print "updates failed to send"
        return
    updates_json = ""
def make_game_dict(data):
    #Make empty game dictonary
    game_dict = {}
    #Fill it with data wanted from the mlb data dictionary
    game_dict["inning"] = data["data"]["game"]["inning"]
    game_dict["home_score"] = data["data"]["game"]["score"]["hr"]
    game_dict["away_score"] = data["data"]["game"]["score"]["ar"]
    game_dict["balls"] = data["data"]["game"]["b"]
    game_dict["strikes"] = data["data"]["game"]["s"]
    game_dict["outs"] = data["data"]["game"]["o"]
    game_dict["at_bat"] = data["data"]["game"]["players"]["batter"]["boxname"]
    game_dict["photo"] = photo + str(data["data"]["game"]["players"]["batter"]["pid"]) + ".jpg"
    return game_dict



if __name__ == "__main__":
    try:
        #Setup Websocket Client, connect to stoh.io:9002 Websocket Server
        factory = WebSocketClientFactory("ws://stoh.io:9002")
        factory.protocol = BroadcastClientProtocol
        connectWS(factory)

        while True:
            #Get json data from url
            json_data = urllib.urlopen(url)
            #Convert json data to Dictionary
            data = json.loads(json_data.read())
            #Convert Dictionary into game_dict
            new_game_dict = make_game_dict(data)
            updates_game_dict = {}
            #iterate through keys in the new data
            for key in new_game_dict:
                if new_game_dict[key] != curr_game_dict[key]:
                    #If they are different than the old data, print it out, update the data, and make a dictonary to send to the server
                    print "new " + key + ": " + new_game_dict[key]
                    curr_game_dict[key] = new_game_dict[key]
                    updates_game_dict[key] = new_game_dict[key]
            if updates_game_dict:
                send_updates(updates_game_dict)

            sleep(3)
    except KeyboardInterrupt:
        exit()
