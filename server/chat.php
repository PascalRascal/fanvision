<?php
use Ratchet\MessageComponentInterface;
use Ratchet\ConnectionInterface;

class Chat implements MessageComponentInterface {
    public $clients;
    private $logs;
    private $connectedUsers;
    private $connectedUsersNames;

    public function __construct() {
        $this->clients = new \SplObjectStorage;
        $this->logs = [];
        $this->connectedUsers = [];
        $this->connectedUsersNames = [];
    }

    public function onOpen(ConnectionInterface $conn) {
        $this->clients->attach($conn);
        echo "New connection! ({$conn->resourceId})\n";
        $conn->send(json_encode($this->logs));
        $this->connectedUsers [$conn->resourceId] = $conn;
    }

    public function onMessage(ConnectionInterface $from, $msg) {
        // Do we have a username for this user yet?
        /*if (isset($this->connectedUsersNames[$from->resourceId])) {
            // If we do, append to the chat logs their message
            $this->logs[] = array(
                "user" => $this->connectedUsersNames[$from->resourceId],
                "msg" => $msg,
                "timestamp" => time()
            );
            $this->sendMessage(end($this->logs));
        } else {
            // If we don't this message will be their username
            $this->connectedUsersNames[$from->resourceId] = $msg;
        }*/
        if ($msg == "game_update")
        {
            $this->logs[] =
                array
                (
                    "data_type" => "game_update",
                    "data" => array
                    (
                        "item" => "strike",
                        "new_value" => 1
                    )
                );
            $this->sendMessage(end($this->logs));
        }
        else if ($msg == "alert")
        {
            $this->logs[] =
                array
                (
                    "data_type" => "alert",
                    "data" => array
                    (
                        "type" => "strike_out"
                    )
                );
            $this->sendMessage(end($this->logs));
        }
        else if ($msg == "new_poi")
        {
            $this->logs[] =
                array
                (
                    "data_type" => "new_poi",
                    "data" => array
                    (
                        "latitude" => 3.5,
                        "longitude" => 4.5,
                        "title" => "blerg",
                        "description" => "blerg is a lot of fun",
                        "color" => "blue"
                    )
                );
            $this->sendMessage(end($this->logs));
        }
    }

    public function onClose(ConnectionInterface $conn) {
        // Detatch everything from everywhere
        $this->clients->detach($conn);
        unset($this->connectedUsersNames[$conn->resourceId]);
        unset($this->connectedUsers[$conn->resourceId]);
    }

    public function onError(ConnectionInterface $conn, \Exception $e) {
        $conn->close();
    }

    private function sendMessage($message) {
        foreach ($this->connectedUsers as $user) {
            $user->send(json_encode($message));
        }
    }
}