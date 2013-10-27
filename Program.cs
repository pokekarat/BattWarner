using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading;

namespace BattApp
{
    class Program
    {
        static void Main(string[] args)
        {   

            int remainBatt = 50;
            bool state = false;
            string battStatus = "";

            while (remainBatt > 1 || remainBatt <100)
            {
                ObjectQuery query = new ObjectQuery("Select EstimatedChargeRemaining, BatteryStatus FROM Win32_Battery");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject mo in collection)
                {
                    foreach (PropertyData property in mo.Properties)
                    {
                        if (property.Value == null) continue;

                        if (property.Name.Contains("EstimatedChargeRemaining"))
                        {
                           // Console.WriteLine("Property {0}: Value is {1}", property.Name, property.Value);
                            remainBatt = int.Parse(property.Value.ToString());
                           
                        }

                        if (property.Name.Contains("BatteryStatus"))
                        {
                            Console.WriteLine(property.Value.ToString());
                            battStatus = property.Value.ToString();
                        }
                    }
                }

                Thread.Sleep(3000);
                Console.WriteLine(remainBatt);

                if ((remainBatt == 10 || remainBatt == 99 ) && !state)
                {
                   
                    Thread playSound = new Thread(()=>PlaySound(battStatus));//new Thread(PlaySound);
                    playSound.Start();

                    state = true;
                }
            }

            //Console.ReadKey();
        }

        static void PlaySound(string bStatus)
        {
            Console.WriteLine("Play sound");

            int i = 0;
            string firstPlay = "";

            if (i == 0)
            {
                i = 1;
                firstPlay = bStatus;
            }
               
            while (true)
            {
                System.Media.SystemSounds.Beep.Play();
                Thread.Sleep(1000);

                if (!firstPlay.Equals(bStatus)) break;

            }
        }
    }
}
