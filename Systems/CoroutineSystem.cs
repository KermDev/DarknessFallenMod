using System;
using System.Collections;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace DarknessFallenMod.Systems
{
    /// <summary>
    /// Unity-like coroutine system by sucss :3
    /// </summary>
    public class CoroutineSystem : ModSystem
    {
        public enum CoroutineType
        {
            PostUpdate
        }

        public override void OnWorldUnload()
        {
            routinesPostUpdate.Clear();
        }

        static readonly List<Coroutine> routinesPostUpdate = new(); 

        public override void PostUpdateEverything()
        {
            for (int i = 0; i < routinesPostUpdate.Count; i++)
            {
                Coroutine routine = routinesPostUpdate[i];

                routine.Update();

                if (!routine.Active)
                {
                    routinesPostUpdate.Remove(routine);
                }
            }
        }

        /// <summary>
        /// What you can return and what it does: <br />
        /// 1. false => Ends the coroutine. <br />
        /// 2. WaitFor.Frames(amount) => Waits for however many frames specified. <br />
        /// 3. Any other object? => Waits one frame (null, true, MyClassBruhXD etc). <br />
        /// <br />
        /// Also when creating a method for this make sure to check for correct Netmode. (ex. Main.netMode == NetmodeID.Server => return false). <br />
        /// </summary>
        /// <param name="enumerator">Method call with IEnumerator type.</param>
        /// <param name="coroutineType">In what type of update should the coroutine tick.</param>
        /// <returns></returns>
        public static Coroutine StartCoroutine(IEnumerator enumerator, CoroutineType coroutineType = CoroutineType.PostUpdate)
        {
            Coroutine routine = new Coroutine(enumerator);
            routine.MoveNext();
            
            switch (coroutineType)
            {
                case CoroutineType.PostUpdate:
                    routinesPostUpdate.Add(routine);
                    break;
            }

            return routine;
        }

        public class Coroutine
        {
            public readonly IEnumerator Enumerator;

            public WaitFor currentWaitFor;
            public bool Active { get; private set; }

            public Coroutine(IEnumerator enumerator)
            {
                Enumerator = enumerator;
                Active = true;
            }

            public void Update()
            {
                object current = Enumerator.Current;

                if (current is bool cBool && !cBool)
                { 
                    Active = false;
                }
                else if (current is null)
                {
                    MoveNext();
                }
                else if (current is WaitFor waitForO)
                {
                    if (currentWaitFor is not null) currentWaitFor.WaitFrames--;
                    else currentWaitFor = WaitFor.Frames(waitForO.WaitFrames - 1);

                    if (currentWaitFor.WaitFrames <= 0) MoveNext();
                }
            }

            public void MoveNext()
            {
                currentWaitFor = null;
                bool finished = !Enumerator.MoveNext();

                if (finished) Active = false;
            }

            public void Stop()
            {
                Active = false;
            }
        }

        public class WaitFor
        {
            public int WaitFrames { get; set; }

            public static WaitFor Frames(int frames)
            {
                return new WaitFor() { WaitFrames = frames };
            }
        }
    }
}
