import urllib
import json
from time import sleep
game_data = {"inning": 0, "home_score": 0, "away_score": 0, "balls": 0, "strikes": 0, "outs": 0, "at_bat": "None", "photo": "None"}
last_game_data = {"inning": -1, "home_score": -1, "away_score": -1, "balls": -1, "strikes": -1, "outs": -1, "at_bat": "-1", "photo": "-1"}
url = "http://gd2.mlb.com/components/game/mlb/year_2016/month_05/day_20/gid_2016_05_20_tbamlb_detmlb_1/plays.json"
data = {}
last_data = data
jsonData_base = {"data_type": "game_update", "data": []}

def make_json_all():
    ## Formats a JSON with all current Game Data.  Send When Client Connects
    jsonData = jsonData_base
    for key, value in game_data.iteritems():
        jsonData["data"].append({"item": key, "new_value": value})
    print "Sending ALL Game Data as JSON"
    print jsonData
    print "------------"
    ##SEND HERE


def update_current_data():
    jsonData = jsonData_base
    global game_data
    global last_game_data
    game_data["inning"] = data["data"]["game"]["inning"]
    game_data["home_score"] = data["data"]["game"]["score"]["hr"]
    game_data["away_score"] = data["data"]["game"]["score"]["ar"]
    game_data["balls"] = data["data"]["game"]["b"]
    game_data["strikes"] = data["data"]["game"]["s"]
    game_data["outs"] = data["data"]["game"]["o"]
    game_data["at_bat"] = data["data"]["game"]["players"]["batter"]["boxname"]
    game_data["photo"] = data["data"]["game"]["players"]["batter"]["pid"]

    if str(game_data) != str(last_game_data):
        for key, value in game_data.iteritems():
            if str(game_data[key]) != str(last_game_data[key]):
                jsonData["data"].append({"item": key, "new_value": value})
        print "Game Update! JSON:"
        print jsonData
        print "-------------"

    last_game_data = game_data

#make_json_all()
while True:
    #global last_data
    #global data
    response = urllib.urlopen(url)
    data = json.loads(response.read())
    if str(last_data) != str(data):
        update_current_data()
    last_data = data
    sleep(3)
