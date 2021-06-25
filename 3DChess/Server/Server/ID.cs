using System;

namespace Server
{
    class ID
    {
        public const int LoginRequest = 1000;
        public const int LoginAnswer = 1001;
        public const int RegisterRequest = 1003;
        public const int RegisterAnswer = 1004;
        public const int MatchRequest = 1005;
        public const int MatchAnswer = 1006;
        public const int Move = 1007;
        public const int Kick = 1002;
        public const int LogoutRequest = 1010;
        public const int LogoutAnswer = 1011;
        public const int InfoRequest = 1008;
        public const int InfoAnswer = 1009;
        public const int OpponentDisconnected = 1012;
        public const int SwitchTurn = 1013;
        public const int LeaveMatch = 1014;
        public const int ChatMessage = 1015;
        public const int Check = 1016;
        public const int Mate = 1017;
        public const int EndMatch = 1018;
        public const int ThreatenNode = 1019;
    }
}
