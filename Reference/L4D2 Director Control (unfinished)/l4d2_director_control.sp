#pragma semicolon 1
#include <sourcemod>
#include <sdktools>
#undef REQUIRE_PLUGIN
#include <adminmenu>

#define PLUGIN_VERSION "0.1"

#define	CLASS_SURVIVOR			0
#define	CLASS_SMOKER			1
#define	CLASS_BOOMER			2
#define	CLASS_HUNTER			3
#define	CLASS_SPITTER			4
#define	CLASS_JOCKEY			5
#define	CLASS_CHARGER			6
#define	CLASS_WITCH				7
#define	CLASS_TANK				8

//Last instated director.
new director = 0;
//Previously instated director.
new old_director = 0;
//Finale has started, and is not in its final stage.
new bool:bFinaleStarted = false;

new bool:bRoundStarted = false;

///Cooldown vars.
//Meter interval. It will update the meter with a timer that uses this delay.
//The amount added every update is also based on this variable.
//In general, it won't add any more or less if you increase or decrease this variable, it will simply be more or less accurate.
new Float:fMeterInterval = 0.25;

new Float:fMeter = 100.0;

new Float:fSurvivorHPMult = 0.0;

//Used for preventing the unfair stacking of specials.
//% of the meter to remove if used.
new Float:fMeterMult[9] = {
	//Mob
	0.6,
	//Smoker
	0.5,
	//Boomer
	0.4,
	//Hunter
	0.5,
	//Spitter
	0.45,
	//Jockey
	0.55,
	//Charger
	0.7,
	//Witch
	0.9,
	//Tank
	1.0
};

public Plugin:myinfo = {
	name = "[L4D2] Player Director",
	author = "#!/trigger_hurt",
	description = "Allows a player to assume the role of \"director\".",
	version = PLUGIN_VERSION,
	url = ""
}

public OnPluginStart() {
	CreateConVar("sm_director_version", PLUGIN_VERSION, "Plugin version", FCVAR_SPONLY|FCVAR_REPLICATED|FCVAR_NOTIFY);
	RegConsoleCmd("direct", Command_BecomeDirector, "Become the director!");
	RegConsoleCmd("sm_director_zombie", Command_PlaceZombie, "Place a zombie.\nFirst argument is type, second is true or false if the game should place the zombie automatically.");
	RegConsoleCmd("sm_director_debug", Command_Debug, "Generic debug command. See the source code for a list of strings this accepts.");
	HookEvent("finale_start", Event_Finale_Start);
	HookEvent("finale_escape_start", Event_Finale_Escape_Start);
	HookEvent("player_left_start_area", Event_Round_Start);
	HookEvent("round_end", Event_Round_End);
	//this timer should not end until the plugin ends.
	CreateTimer(fMeterInterval, MeterTimer, _, TIMER_REPEAT);
}

public Action:Event_Finale_Start(Handle:event, String:event_name[], bool:dontBroadcast) bFinaleStarted = true;
public Action:Event_Finale_Escape_Start(Handle:event, String:event_name[], bool:dontBroadcast) bFinaleStarted = false;
public Action:Event_Round_Start(Handle:event, String:event_name[], bool:dontBroadcast) {
	if (director > 0) PrintToChat(director, "ROUND START!");
	bRoundStarted = true;
}
public Action:Event_Round_End(Handle:event, String:event_name[], bool:dontBroadcast) bRoundStarted = false;

stock WarningNotDirector(client) PrintToChat(client, "Only the director can run this command.");

public Action:MeterTimer(Handle:timer) {
	if (bRoundStarted) {
		new Float:fSurvivorHPAvg = AverageSurvivorHP();
		fSurvivorHPMult = 2.0 - (fSurvivorHPAvg / 50.0);
		//fMeter = fMeter + ((fSurvivorHPMult * fMeterInterval) * ((bFinaleStarted) ? 1.5 : 1.0));
		fMeter += ((bFinaleStarted) ? 7.5 : 5.0);
	} else fMeter = 0.0;
	//obviously, can't print hint text to the AI director.
	if (director > 0) PrintHintText(director, "[METER | %f percent]", fMeter);
}

public Action:Command_Debug(client, args) {
	decl String:arg[32];
	GetCmdArg(1, arg, sizeof(arg));
	if (client != director) {
		WarningNotDirector(client);
		return;
	}
	if (StrEqual(arg, "", false)) {
		PrintToChat(director, "You must supply an argument.");
	} else if (StrEqual(arg, "finale", false)) {
		PrintToChat(director, "The finale has %sstarted.", (bFinaleStarted) ? "" : "not ");
	} else if (StrEqual(arg, "round", false)) {
		PrintToChat(director, "The round has %sstarted.", (bRoundStarted) ? "" : "not ");
	} else if (StrEqual(arg, "director", false)) {
		PrintToChat(director, "%N is the current director.", director);
	} else if (StrEqual(arg, "meter", false)) {
		PrintToChat(director, "The meter is at %f percent.\nIt updates %f times every second.\nThe current HP multiplier is %f.", fMeter, (1.0 / fMeterInterval), fSurvivorHPMult);
	}
}

stock WarningMeterInsufficient(Float:requires) {
	PrintToChat(director, "The meter must be at least %f percent full to spawn that.", requires);
}

public Action:Command_PlaceZombie(client, args) {
	decl String:arg[8];
	GetCmdArg(1, arg, sizeof(arg));
	decl String:cursor[8];
	GetCmdArg(2, cursor, sizeof(cursor));

	if (client != director) {
		WarningNotDirector(client);
		return;
	} else if (!bRoundStarted) {
		PrintToChat(director, "The round has not yet started.");
		return;
	}
	if (StrEqual(arg, "", false)) {
		PrintToChat(director, "Doesn't work yet.");
	} else if (StrEqual(arg, "horde", false)) {
		PrintToChat(director, "Doesn't work yet.");
	} else if (StrEqual(arg, "hunter", false)) {
		new Float:fRequirement = (fMeterMult[CLASS_HUNTER] * 100.0);
		if (fMeter < fRequirement) {
			WarningMeterInsufficient(fRequirement);
		} else {
			new zombie = CreateFakeClient("Hunter");
			SpawnInfected(zombie, 3, StrEqual(cursor, "true", false));
			fMeter = fMeter - fRequirement;
		}
	} else if (StrEqual(arg, "boomer", false)) {
		new Float:fRequirement = (fMeterMult[CLASS_BOOMER] * 100.0);
		if (fMeter < fRequirement) {
			WarningMeterInsufficient(fRequirement);
		} else {
			new zombie = CreateFakeClient("Boomer");
			SpawnInfected(zombie, 2, StrEqual(cursor, "true", false));
			fMeter = fMeter - fRequirement;
		}
	} else if (StrEqual(arg, "smoker", false)) {
		new Float:fRequirement = (fMeterMult[CLASS_SMOKER] * 100.0);
		if (fMeter < fRequirement) {
			WarningMeterInsufficient(fRequirement);
		} else {
			new zombie = CreateFakeClient("Smoker");
			SpawnInfected(zombie, 1, StrEqual(cursor, "true", false));
			fMeter = fMeter - fRequirement;
		}
	} else if (StrEqual(arg, "charger", false)) {
		new Float:fRequirement = (fMeterMult[CLASS_CHARGER] * 100.0);
		if (fMeter < fRequirement) {
			WarningMeterInsufficient(fRequirement);
		} else {
			new zombie = CreateFakeClient("Charger");
			SpawnInfected(zombie, 6, StrEqual(cursor, "true", false));
			fMeter = fMeter - fRequirement;
		}
	} else if (StrEqual(arg, "jockey", false)) {
		new Float:fRequirement = (fMeterMult[CLASS_JOCKEY] * 100.0);
		if (fMeter < fRequirement) {
			WarningMeterInsufficient(fRequirement);
		} else {
			new zombie = CreateFakeClient("Jockey");
			SpawnInfected(zombie, 5, StrEqual(cursor, "true", false));
			fMeter = fMeter - fRequirement;
		}
	} else if (StrEqual(arg, "spitter", false)) {
		new Float:fRequirement = (fMeterMult[CLASS_SPITTER] * 100.0);
		if (fMeter < fRequirement) {
			WarningMeterInsufficient(fRequirement);
		} else {
			new zombie = CreateFakeClient("Spitter");
			SpawnInfected(zombie, 4, StrEqual(cursor, "true", false));
			fMeter = fMeter - fRequirement;
		}
	} else if (StrEqual(arg, "tank", false)) {
		if (bFinaleStarted) {
			PrintToChat(director, "A finale is in-progress, you can't spawn tanks until the very end.");
		} else {
			new Float:fRequirement = (fMeterMult[CLASS_TANK] * 100.0);
			if (fMeter < fRequirement) {
				WarningMeterInsufficient(fRequirement);
			} else {
				new zombie = CreateFakeClient("Tank");
				SpawnInfected(zombie, 8, StrEqual(cursor, "true", false));
				fMeter = fMeter - fRequirement;
			}
		}
	} else if (StrEqual(arg, "witch", false)) {
		if (bFinaleStarted) {
			PrintToChat(director, "You cannot spawn a witch during a finale.");
		} else {
			PrintToChat(client, "Doesn't work yet.");
		}
	}
}

//Anyone should be able to run this command.
public Action:Command_BecomeDirector(client, args) {
	//no args for this function.
	if (director == client) {
		//client is already the director; assume they want to leave.
		old_director = director;
		director = 0;
		OnDirectorTakeover();
	} else {
		//Later on, be sure to introduce a system to let the client who's currently directing be asked if they want to step down.
		if (director != 0) PrintToChat(client, "There's already a non-AI director.\nRevoking their seat.");
		old_director = director;
		director = client;
		OnDirectorTakeover();
	}
}

stock OnDirectorTakeover() {
	//if a director is '0', then it's the AI director.
	if (director == old_director) {
		//director attempted to reinstate himself.
		if (director != 0) PrintToChat(director, "You are already the director!");
		return;
	}
	for (new i = 1; i <= MaxClients; i++) {
		if (i == director) {
			if (old_director == 0) {
				//AI is no longer directing.
				PrintToChat(director, "You have assumed the role of the director!");
			} else {
				PrintToChat(director, "You have assumed %N's role as the director!", old_director);
			}
			//Make the new director a spectator.
			ChangeClientTeam(director, 1);
		} else if (i == old_director) {
			if (director == 0) {
				//You are no longer directing.
				PrintToChat(old_director, "The server has assumed your role as the director!");
			} else {
				PrintToChat(old_director, "%N has assumed your role as the director!", director);
			}
			//Trying to put the old director back into the survivors. If not possible, they join the infected team instead.
			///CURRENTLY BROKEN.
			PrintToChat(old_director, "%N, the team changing function does NOT function for spectators.\nIf you want to play the game, you will have to run the jointeam command yourself.", old_director);
			new bots = 0;
			for (new j = 1; j <= MaxClients; j++) if (IsSurvivor(j) && IsFakeClient(j)) bots++;
			if (bots == 0) {
				PrintToChat(old_director, "There are no survivor bots for you to become.\nYou should instead become a zombie.");
				ChangeClientTeam(old_director, 3);
			} else {
				ChangeClientTeam(old_director, 2);
				for (new j = 1; j <= MaxClients; j++) {
					decl String:j_mdl[128];
					GetClientModel(j, j_mdl, sizeof(j_mdl));
					if (IsSurvivor(j) && IsFakeClient(j)) {
						if (StrContains(j_mdl, "coach", false) != -1) ClientCommand(old_director, "jointeam 2 coach");
						else if (StrContains(j_mdl, "gambler", false) != -1) ClientCommand(old_director, "jointeam 2 nick");
						else if (StrContains(j_mdl, "mechanic", false) != -1) ClientCommand(old_director, "jointeam 2 ellis");
						else if (StrContains(j_mdl, "producer", false) != -1) ClientCommand(old_director, "jointeam 2 rochelle");
						else if (StrContains(j_mdl, "biker", false) != -1) ClientCommand(old_director, "jointeam 2 francis");
						else if (StrContains(j_mdl, "manager", false) != -1) ClientCommand(old_director, "jointeam 2 louis");
						else if (StrContains(j_mdl, "namvet", false) != -1) ClientCommand(old_director, "jointeam 2 bill");
						else if (StrContains(j_mdl, "teenangst", false) != -1) ClientCommand(old_director, "jointeam 2 zoey");
					}
				}
			}
		} else {
			if (director == 0) {
				//Tell everyone else about the return of the AI director.
				PrintToChat(i, "The server has assumed %N's role as the director!", old_director);
			} else if (old_director == 0) {
				//Tell everyone else about the new director.
				PrintToChat(i, "%N has assumed the role of the director!", director);
			} else PrintToChat(i, "%N has assumed %N's role as the director!", director, old_director);
		}
	}
	//There's actually nothing to change here, because all of the director commands are specific to whatever client is in the director variable.
	//New director is the AI director.
	if (director == 0) PrintToServer("The AI director has resumed his role as the director.");
	//Old director wasn't a client.
	else if (old_director == 0) PrintToServer("%N has relieved the AI director of his role as the director.", director);
	//New director is a client, and so was the old director.
	else PrintToServer("%N has relieved %N of their role as the director.", director, old_director);
	//Suppress a warning during compiling that says this should return a value.
	return;
}

stock AverageSurvivorPositions() {
	//function basically just gets the averaged, and is used to calculate the finalized distance limits.
	//it returns a vector, which is a 3 floating-point array.
	//first, store the survivor's world vectors.
	new Float:survivor_coords_average[3], Float:survivor_coords_added[3], Float:survivor_coords_tmp[3];
	new Float:survivors = 0.0;
	for (new i = 1; i <= MaxClients; i++) {
		if (IsSurvivor(i) && IsAlive(i)) {
			survivors = survivors + 1.0;
			GetEntPropVector(i, Prop_Send, "m_vecOrigin", survivor_coords_tmp);
			//add the values grabbed from the live survivor to the stack of coordinates to average.
			for (new o = 0; o < 3; o++) survivor_coords_added[o] = survivor_coords_added[o] + survivor_coords_tmp[o];
		}
	}
	for (new i = 0; i < 3; i++) survivor_coords_average[i] = survivor_coords_added[i] / survivors
	//the survivor_coords_average array now contains an average world coordinate to use as the center for allowing/disallowing spawns based on distance.
	//or, in a nutshell, this is where the center of the sphere that represents the "no-spawn" zone is.
	return survivor_coords_average;
}

//returns an int between 0 (all dead) and 100 (all at 100% hp);
stock AverageSurvivorHP() {
	new Float:survivor_hp_total = 0.0,
		Float:survivors = 0.0;
	for (new i = 1; i <= MaxClients; i++) {
		if (IsSurvivor(i)) {
			survivors = survivors + 1.0;
			new Float:survivor_hp = GetClientRealHealth(i);
			//clamp hp to 100 max. there's a bug where it's possible to use some water and pills to get well above 100 hp.
			if (survivor_hp > 100.0) survivor_hp = 100.0;
			survivor_hp_total = survivor_hp_total + survivor_hp;
		}
	}
	return (survivor_hp_total / survivors);
}

//I created this function. It's designed to be more consistent than simply getting their team, which doesn't change when switching roles.
//Specifically, in anything not based on versus mode, team 2 is always the survivors, and team 3 is always the infected.
//HOWEVER, in anything based on versus mode, when the teams switch after one succeeds/fails, team 2 becomes the infected, and team 3 becomes the survivors.
//This means that the team number is reliable for knowing whether or not someone is a survivor.
//This function remedies this by simply asking what their zombie class is. Zombie classes that can be found on player entities will be between 1 and 8.
stock bool:IsSurvivor(client) {
	new zombie_class = GetEntData(client, FindSendPropInfo("CTerrorPlayer", "m_zombieClass"));
	//Zombie classes are WITHIN 1 and 8. Survivors seem to use either class 0 or 9. 0 might be for commons.
	if (zombie_class > 0 && zombie_class < 9) return false;
	else return true;
}
stock bool:IsInfectedClass(client, type=0) {
	//Use CLASS_<x> as the second arg.
	new zombie_class = GetEntData(client, FindSendPropInfo("CTerrorPlayer", "m_zombieClass"));
	//Zombie classes are WITHIN 1 and 8. Survivors seem to use either class 0 or 9. 0 might be for commons.
	if (zombie_class == type) return true;
	else return false;
}

stock SpawnInfected(client, Class, bool:bCursor=false) {
	ChangeClientTeam(client, 3);
	new String:g_sBossNames[9+1][10]={"","smoker","boomer","hunter","spitter","jockey","charger","witch","tank","survivor"};
	decl String:options[30];
	if (Class < 1 || Class > 8) return false;
	if (GetClientTeam(client) != 3) return false;
	if (!IsClientInGame(client)) return false;
	if (IsPlayerAlive(client)) return false;

	Format(options,sizeof(options),"%s auto", g_sBossNames[Class]);
	CheatCommand(client, "z_spawn_old", options);
	if (bCursor) {
		decl Float:start[3], Float:angle[3], Float:end[3];
		GetClientEyePosition(client, start);
		GetClientEyeAngles(client, angle);
		TR_TraceRayFilter(start, angle, MASK_SOLID, RayType_Infinite, TraceEntityFilterPlayer, client);
		if (TR_DidHit(INVALID_HANDLE)) {
			TR_GetEndPosition(end, INVALID_HANDLE);
		}
		SetEntPropVector(client, Prop_Send, "m_vecOrigin", end);
	}
	
	if (IsFakeClient(client)) {
		KickClient(client);
	}
	return true;
}

stock CheatCommand(client, const String:command[], const String:arguments[]) {
	new flags = GetCommandFlags(command);
	SetCommandFlags(command, flags & ~FCVAR_CHEAT);
	FakeClientCommand(client, "%s %s", command, arguments );
	SetCommandFlags(command, flags | FCVAR_CHEAT);
}
stock CheatServerCommand(const String:command[]) {
	new flags = GetCommandFlags(command);
	SetCommandFlags(command, flags & ~FCVAR_CHEAT);
	ServerCommand(command);
	SetCommandFlags(command, flags | FCVAR_CHEAT);
}

public bool:TraceEntityFilterPlayer(entity, contentsMask, any:data) return entity > MaxClients;

stock GetClientRealHealth(client) {
	if(!client
	|| !IsValidEntity(client)
	|| !IsClientInGame(client)
	|| IsClientObserver(client)) {
		return -1.0;
	}
	if (!IsPlayerAlive(client)) return 0.0;
	if(GetClientTeam(client) != 2) return GetEntPropFloat(client, Prop_Send, "m_iHealth");
	new Float:buffer = GetEntPropFloat(client, Prop_Send, "m_healthBuffer");
	new Float:TempHealth;
	new Float:PermHealth = GetEntPropFloat(client, Prop_Send, "m_iHealth");
	if(buffer <= 0.0) TempHealth = 0.0;
	else {
		new Float:difference = GetGameTime() - GetEntPropFloat(client, Prop_Send, "m_healthBufferTime");
		new Float:decay = GetConVarFloat(FindConVar("pain_pills_decay_rate"));
		new Float:constant = 1.0/decay;
		TempHealth = buffer - (difference / constant);
	}
	if (TempHealth < 0.0) TempHealth = 0.0;
	return PermHealth + TempHealth;
}

