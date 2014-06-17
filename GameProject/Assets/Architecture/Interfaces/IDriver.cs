﻿using Behaviours;
using Cars;

namespace Interfaces
{
    public interface IDriver
    {
        PlayerAction GetPlayerAction();
		void HandlePlayerAction(AutoBehaviour autoBehaviour);
    }
}