<?php
require 'vendor/autoload.php';
use Ratchet\MessageComponentInterface;
use Ratchet\ConnectionInterface;

require 'chat.php';
$loop = React\EventLoop\Factory::create();


echo "1created socket connection";
// Run the server application through the WebSocket protocol on port 8080
$app = new Ratchet\App("localhost", 8080, '0.0.0.0', $loop);
echo "2created socket connection";
$app->route('/chat', new Chat, array('*'));
echo "3created socket connection";

$app->run();
echo "c4reated socket connection";
