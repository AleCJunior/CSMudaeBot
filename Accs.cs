namespace MudaeBOT
{
    // list [group] [account] . [0: Id / 1: Nick]

    internal class AccKeys
    {
        public static DiscordAcc[][] list = new DiscordAcc[][]
        {
            new DiscordAcc[] // group
            {
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 1 Acc 1
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 1 Acc 2
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 1 Acc 3
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 1 Acc 4
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" } // Your account Group 1 Acc 5
            },

            // =======================================================
            new DiscordAcc[]
            {
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 2 Acc 1
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 2 Acc 2
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 2 Acc 3
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 2 Acc 4
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" } // Your account Group 2 Acc 5
            },

            // =======================================================
            new DiscordAcc[]
            {
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 3 Acc 1
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 3 Acc 2
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 3 Acc 3
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" }, // Your account Group 3 Acc 4
                new DiscordAcc { Id = "000000000000000000", Nick = "Account" } // Your account Group 3 Acc 5
            }
        };

        // Each account group can have their own channel to do the diag command
        public static string[] diagChannel = new string[]
        {
            "0000000000000000000",
            "0000000000000000000",
            "0000000000000000000"
        };
    }
}
