###############################################################################
#
# The MIT License (MIT)
#
# Copyright (c) Tavendo GmbH
#
# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:
#
# The above copyright notice and this permission notice shall be included in
# all copies or substantial portions of the Software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
# THE SOFTWARE.
#
###############################################################################
import json
import ast
from twisted.internet import reactor
from twisted.python import log
from twisted.web.server import Site
from twisted.web.static import File

from autobahn.twisted.websocket import WebSocketServerFactory, \
    WebSocketServerProtocol, \
    listenWS


currData = {"inning": -1, "home_score": -1, "away_score": -1, "balls": -1, "strikes": -1, "outs": -1, "at_bat": "-1", "photo": "-1"}
jsonData_base = {"data_type": "game_update", "data": []}

class BroadcastServerProtocol(WebSocketServerProtocol):
    global myVar
    # global currData

    def onConnect(self, request):
        print("Client connecting: {0}".format(request.peer))

    def onOpen(self):
        print("WebSocket connection open.")
        #self.sendMessage(myVar)

    def onMessage(self, payload, isBinary):
        print "NEW MESSAGE"
        if isBinary:
            print("Binary message received: {0} bytes".format(len(payload)))
        else:
            # print("Text message received: {0}".format(payload.decode('utf8')))
            text = format(payload.decode('utf8'))
            #global jsonData_base
            if text[0:6] != 'server':
                print "NOT FROM SERVER"
                global currData
                global jsonData
                jsonData  = jsonData_base
                print "JSON DATA" + str(jsonData_base)

                for key, value in currData.iteritems():
                    jsonData["data"].append({"item": key, "new_value": value})
                print "SENDING: " + str(json.dumps(jsonData))
                self.sendMessage(json.dumps(jsonData), isBinary)
                jsonData = None
                jsonData_base = {"data_type": "game_update", "data": []}

            else:
                print "FROM SERVER"
                global jsonData_base
                jsonData = jsonData_base
                global currData
                if currData["inning"] == -1:
                    currData = ast.literal_eval(text[6:])
                else:
                    data = currData
                    data = ast.literal_eval(text[6:])
                    for key, value in data.iteritems():
                        global currData
                        currData[key] = value
                        jsonData["data"].append({"item": key, "new_value": value})
                    print "BROADCASTING: " + str(json.dumps(jsonData))
                    self.factory.broadcast(json.dumps(jsonData))
                    jsonData_base = {"data_type": "game_update", "data": []}






    def onClose(self, wasClean, code, reason):
        print("WebSocket connection closed: {0}".format(reason))



class BroadcastServerFactory(WebSocketServerFactory):

    """
    Simple broadcast server broadcasting any message it receives to all
    currently connected clients.
    """

    def __init__(self, url):
        WebSocketServerFactory.__init__(self, url)
        self.clients = []

    def register(self, client):
        if client not in self.clients:
            print("registered client {}".format(client.peer))
            self.clients.append(client)

    def unregister(self, client):
        if client in self.clients:
            print("unregistered client {}".format(client.peer))
            self.clients.remove(client)

    def broadcast(self, msg):
        print("broadcasting message '{}' ..".format(msg))
        for c in self.clients:
            c.sendMessage(msg.encode('utf8'))
            print("message sent to {}".format(c.peer))


class BroadcastPreparedServerFactory(BroadcastServerFactory):

    """
    Functionally same as above, but optimized broadcast using
    prepareMessage and sendPreparedMessage.
    """

    def broadcast(self, msg):
        print("broadcasting prepared message '{}' ..".format(msg))
        preparedMsg = self.prepareMessage(msg)
        for c in self.clients:
            c.sendPreparedMessage(preparedMsg)
            print("prepared message sent to {}".format(c.peer))
















if __name__ == '__main__':

    import sys

    from twisted.python import log
    from twisted.internet import reactor

    log.startLogging(sys.stdout)
    ServerFactory  = BroadcastServerFactory

    factory = ServerFactory(u"ws://127.0.0.1:9002")
    factory.protocol = BroadcastServerProtocol
    # factory.setProtocolOptions(maxConnections=2)

    reactor.listenTCP(9002, factory)
    reactor.run()
