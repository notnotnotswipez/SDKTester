using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTester.ActionQueuer
{
    public class ActionQueuer
    {
        private static List<Action> actions = new List<Action>();
        private static List<Action> secondaryActions = new List<Action>();

        public static void SecondaryQueueAction(Action action)
        {
            secondaryActions.Add(action);
        }

        public static void QueueAction(Action action) {
            actions.Add(action);
        }

        public static void RunSecondaryActions()
        {
            foreach (Action action in secondaryActions)
            {
                action.Invoke();
            }

            secondaryActions.Clear();
        }

        public static void RunActions() {
            foreach (Action action in actions)
            {
                action.Invoke();
            }

            actions.Clear();
        }
    }
}
