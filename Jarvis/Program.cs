using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace Jarvis
{
    class Program
    {
        //This will greet the user in default voice
        private static SpeechSynthesizer synth = new SpeechSynthesizer(); 

        static void Main(string[] args)
        {
            
            synth.Speak("Welcome to Jarvis");

            #region My Performance Counters
            //This will load the current CPU load in percentage
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfCpuCount.NextValue();

            //This will load the current available memory in MB
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            perfMemCount.NextValue();

            //This will load the system uptime(in seconds)
            PerformanceCounter perfSysUpCount = new PerformanceCounter("System", "System Up Time");
            perfSysUpCount.NextValue();
            #endregion

            TimeSpan uptimeSpan = TimeSpan.FromSeconds(perfSysUpCount.NextValue());
            string systemTimeUpVocalMessage = string.Format("The current system time up is {0} days {1} hours {2} minutes {3} seconds",
                (int)uptimeSpan.TotalDays,
                (int)uptimeSpan.Hours,
                (int)uptimeSpan.Minutes,
                (int)uptimeSpan.Seconds
            );

            //Tell the system what current system uptime is
            Speak(systemTimeUpVocalMessage, VoiceGender.Male, 3);

            int speechSpeed = 1;

            //Infinite Loop
            while (true)
            {
                //Get the current performance counter value
                int currentCpuPercentage = (int)perfCpuCount.NextValue();
                int currentAvailableMemory = (int)perfMemCount.NextValue();
               
                //After every 1 second print the CPU load in percentage to the screen
                Console.WriteLine("CPU Load: {0}%", currentCpuPercentage);
                Console.WriteLine("Available Memory: {0}MB", currentAvailableMemory);

                //Speak the current values of performance counters.
                if(currentCpuPercentage > 80)
                {
                    if(currentCpuPercentage == 100)
                    {
                        if(speechSpeed < 5)
                        {
                            speechSpeed++;
                        }
                        string cpuLoadVocalMessage = string.Format("WARNING: Your CPU will catch fire, kill some programs to avoid");
                        Speak(cpuLoadVocalMessage, VoiceGender.Female, speechSpeed);
                    }
                    else
                    {
                        string cpuLoadVocalMessage = string.Format("The current CPU load is {0}%", currentCpuPercentage);
                        Speak(cpuLoadVocalMessage, VoiceGender.Male, 5);
                    }
                    
                }

                if (currentAvailableMemory < 500)
                {
                    string memAvailablelVocalMessage = string.Format("You have {0} megabytes of available memory", currentAvailableMemory);
                    Speak(memAvailablelVocalMessage, VoiceGender.Male, 5);
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Speaks with a selected voice and message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="vg"></param>
        public static void Speak(string message, VoiceGender vg)
        {
            synth.SelectVoiceByHints(vg);
            synth.Speak(message);
        } 

        /// <summary>
        /// Speaks with a selected voice and message at a selected rate
        /// </summary>
        /// <param name="message"></param>
        /// <param name="vg"></param>
        /// <param name="rate"></param>
        public static void Speak(string message, VoiceGender vg, int rate)
        {
            synth.Rate = rate;
            Speak(message, vg);
        }
    }
}
