﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Aliases
    {
        public Aliases(int Num, String N, String I)
        {
            Number = Num;
            Names = N.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            IPS = new List<String>(I.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public List<String> Names { get; private set; }
        public List<String> IPS { get; private set; }
        public int Number { get; private set; }
    }

    public class Player
    {
        public enum Permission
        {
            Banned = -1,
            User = 0,
            Flagged = 1,
            Trusted = 2,
            Moderator = 3,
            Administrator = 4,
            SeniorAdmin = 5,
            Owner = 6,
            Creator = 7,
            Console = 8,
        }

        public override bool Equals(object obj)
        {
            return ((Player)obj).npID == this.npID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Player(string n, string id, int num, int l)
        {
            Name = n;
            npID = id;
            clientID = num;
            Level = (Player.Permission)l;
            lastOffense = String.Empty;
            Connections = 0;
            IP = "";
            Warnings = 0;
            Alias = new Aliases(0, "", "");
            LastConnection = DateTime.Now;

        }

        public Player(string n, string id, int num, String I)
        {
            Name = n;
            npID = id;
            clientID = num;
            IP = I;
            LastConnection = DateTime.Now;
        }

        public Player(String n, String id, Player.Permission P, String I, String UID)
        {
            Name = n;
            npID = id;
            Level = P;
            IP = I;
            clientID = -1;
            this.UID = UID;
        }

        public Player(string n, string id, int num, Player.Permission l, int cind, String lo, int con, String IP2)
        {
            Name = n;
            npID = id;
            clientID = num;
            Level = l;
            databaseID = cind;
            if (lo == null)
                lastOffense = String.Empty;
            else
                lastOffense = lo;
            Connections = con;
            IP = IP2;
            Warnings = 0;
            Masked = false;
            LastConnection = DateTime.Now;
        }

        public Player(string n, string id, int num, Player.Permission l, int cind, String lo, int con, String IP2, DateTime LC, string UID, bool masked)
        {
            Name = n;
            npID = id;
            clientID = num;
            Level = l;
            databaseID = cind;
            if (lo == null)
                lastOffense = String.Empty;
            else
                lastOffense = lo;
            Connections = con;
            IP = IP2;
            Warnings = 0;
            Masked = false;
            LastConnection = LC;
            this.UID = UID.Trim();
            Masked = masked;
        }

        public bool registerUID(String UID)
        {
            if (UID.Length > 5)
                this.UID = UID;

            return this.UID == UID;
        }

        public String getLastConnection()
        {
            return Utilities.timePassed(LastConnection);
        }

        public void updateName(String n)
        {
            if (n.Trim() != String.Empty)
                Name = n;
        }

        public void updateIP(String I)
        {
            IP = I;
        }

        public void setLevel(Player.Permission Perm)
        {
            Level = Perm;
        }

        public async Task Tell(String Message)
        {
            await lastEvent.Owner.Tell(Message, this);
        }

        public async Task Kick(String Message, Player Sender)
        {
            await lastEvent.Owner.Kick(Message, this, Sender);
        }

        public async Task TempBan(String Message, Player Sender)
        {
            await lastEvent.Owner.TempBan(Message, this, Sender);
        }

        public async Task Warn(String Message, Player Sender)
        {
            await lastEvent.Owner.Warn(Message, this, Sender);
        }

        public async Task Ban(String Message, Player Sender)
        {
            await lastEvent.Owner.Ban(Message, this, Sender);
        }

        public String Name { get; private set; }
        public string npID { get; private set; }
        public int clientID { get; private set; }
        public Player.Permission Level { get; private set; }
        public int databaseID { get; private set; }
        public int Connections { get; set; }
        public String IP { get; private set; }
        public String UID { get; private set; }
        public DateTime LastConnection { get; private set; }
        public Server currentServer { get; private set; }

        public int Ping;

        public Event lastEvent;
        public String lastOffense;
        public int Warnings;
        public Aliases Alias;
        public bool Masked;
        public int selectedServer;
    }
}
