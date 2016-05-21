#!/usr/bin/python
from sys import stdout
from time import sleep
import random
strikes = 0
away = 0
home = 0
def roll_dice():
	global home
	global away
	dice = random.randint(1,10)

	if dice < 7:
		add_strike()
		msg = '{"data_type": "game_update", "data": [{"item": "strikes", "new_value": ' + str(strikes) + '}]}'
	elif  6 < dice < 8:
		away += 1
		msg = '{"data_type": "game_update", "data": [{"item": "away_score", "new_value": ' + str(away) + '}]}'
	else:
		home += 1
		msg = '{"data_type": "game_update", "data": [{"item": "home_score", "new_value": ' + str(home) + '}]}' 
		send_msg('{"data_type": "alert", "data": {"type": "homerun"}}')
		sleep(1)
	if not msg == "": send_msg(msg)	

def send_msg(msg):
	print(msg)
	stdout.flush()


def add_strike():
	global strikes
	if strikes < 3: strikes += 1
	else: strikes = 0

send_msg('{"data_type": "new_poi", "data": {"latitude": 41.1684462, "longitude": -81.3993788, "title": "High School", "description": "Home of the BullDogs", "color": "blue"}}')
sleep(1)
try:
	while True:
		roll_dice()
		sleep(5)
except KeyboardInterrupt:
	pass
