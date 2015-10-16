//
// CST 276 - Assignment #1
//
// Author:  Patrick Carlson
//
// File:  Assignment1
//
// Morg simulator. Utilizes strategy pattern and observer pattern
// to define and maintain morgs.
//
// Main creates morgs which are then fed into the simulator.
//

using System;
using System.Collections.Generic;


namespace Assignment_1
{
    class Program
    {
        static void Main(string[] args)
        {
            A Morg1 = new A("Morg1");

            B Morg2 = new B("Morg2");

            C Morg3 = new C("Morg3");

            Simulator sim1 = new Simulator(Morg1, Morg2, Morg3);

            sim1.run();
        }
    }

    //
    //Morg Observer interface
    //
    interface IMorgObservers
    {
        void UpdateLocation(int newX, int newY);
        void UpdateLiving();
    }

    //
    //Morg class, inherits from Morg Observer interface
    //
    abstract class Morg : IMorgObservers
    {
        protected IMoveBehavior moveBehavior;
        protected IFeedBehavior feedBehavior;

        private string name;
        private bool living;
        private int[] location;
        private List<IMorgObservers> observers;
        private Morg prey;
        private bool isfollowing;
        protected char morgType;

        //
        //Morg constructor
        //
        public Morg(string newName)
        {
            observers = new List<IMorgObservers>();
            living = true;
            name = newName;
            isfollowing = false;

            location = new int[2];

            Random rnd = new Random();
            int locX = rnd.Next(0, 9);
            int locY = rnd.Next(0, 9);
            location[0] = locX;
            location[1] = locY;
        }

        //
        //SetLocation 
        //
        public void setLocation(int X, int Y)
        {
            location[0] = X;
            location[1] = Y;
        }

        //
        //Living status bool accessor
        //
        public bool getLiving() { return living;  }

        //
        //Location accessor
        //
        public int[] getLocation() { return location;  }

        //
        //Morg name accessor
        //
        public string getName() { return name;  }

        //
        //Update location for observers
        //
        public void UpdateLocation(int newX, int newY)
        {
            location[0] = newX;
            location[1] = newY;
        }

        //
        //Returns bool value indicating if morg is following another morg
        //
        public bool getFollowstatus()
        {
            return isfollowing;
        }

        //
        //Update bool of living status, update all observers of living status.
        //
        public void UpdateLiving()
        {
            foreach (IMorgObservers element in observers)
                UpdateLiving();

            living = false;
        }

        //
        //Morgtype accessor
        //
        public char getMorgtype()
        {
            return morgType;
        }

        //
        //Move update member function, will exhibit inidividual morg move behavior
        //
        public void Move(int moveX, int moveY)
        {
            if (moveX < 0 || moveX > 9 || moveY < 0 || moveY > 9)
                Console.WriteLine(name + " runs into a wall and does not move!");

            else
            {

                Console.WriteLine(name + " will");
                moveBehavior.move();
                Console.WriteLine("to " + moveX + " " + moveY + ".");

                foreach (IMorgObservers element in observers)
                {
                    UpdateLocation(moveX, moveY);
                }
            }
            
        }


        //
        //Assigns prey to Morg if non null value prey passed in
        //
        public void setPrey(Morg newPrey)
        {
            prey = newPrey;
            if (newPrey != null)
            {
                isfollowing = true;
                prey.registerObserver(this);
            }

            else
                isfollowing = false;
        }

        //
        //Current prey accessor
        //
        public Morg getPrey()
        {
            return prey;
        }

        //
        //feed behaviour for morg class
        //
        public void feed(Morg prey)
        {
            Console.WriteLine( name + " will" );
            feedBehavior.feed();
            Console.WriteLine(prey.name + "!");
        }

        //
        //add morg to currents list of morgs observing(hunting)
        //
        public void registerObserver(IMorgObservers o)
        {
            observers.Add(o);
            
        }
    }

    //
    //Morg child class A
    //
    class A : Morg
    {
        public A(string newName) : base(newName)
        {
            morgType = 'A';
            feedBehavior = new feedAbsorb();
            moveBehavior = new movePaddle();
        }
    }

    //
    //Morg child class B
    //
    class B : Morg
    {
        public B(string newName) : base(newName)
        {
            morgType = 'B';
            feedBehavior = new feedEnvelop();
            moveBehavior = new moveOoze();
        }
    }

    //
    //Morg child class C
    //
    class C : Morg
    {
        public C(string newName) : base(newName)
        {
            morgType = 'C';
            feedBehavior = new feedEnvelop();
            moveBehavior = new movePaddle();
        }

    }

    //
    //Movebehavior interface
    //
    interface IMoveBehavior
    {
        void move();
    }

    //
    //move behavior
    //
    class movePaddle : IMoveBehavior
    {
        public void move()
        {
            Console.WriteLine("paddle");
        }
    }

    //
    //move behavior
    //
    class moveOoze : IMoveBehavior
    {
        public void move()
        {
            Console.WriteLine("ooze");
        }
    }

    //
    //feed behavior interface
    //
    interface IFeedBehavior
    {
        void feed();
    }

    //
    //feed behavior
    //
    class feedAbsorb : IFeedBehavior
    {
        public void feed()
        {
            Console.WriteLine("absorb");
        }
    }

    //
    //feed behavior
    //
    class feedEnvelop : IFeedBehavior
    {
        public void feed()
        {
            Console.WriteLine("envelope");
        }
    }

    //
    //Simulator class will perform moves and checks for hunting and eating
    //
    class Simulator
    {
        //
        //List of morgs in simulator
        //
        private List<Morg> morgList;
        
        //
        //Simulator constructor
        //
        public Simulator(Morg morg1, Morg morg2, Morg morg3)
        {
            morgList = new List<Morg>();
            morgList.Add(morg1);
            morgList.Add(morg2);
            morgList.Add(morg3);
            Random rnd = new Random();
            foreach ( Morg element in morgList)
            {
                int locX = rnd.Next(0, 9);
                int locY = rnd.Next(0, 9);

                element.setLocation(locX, locY);
            }
        }

        //
        // run member function, allows for adjustment of number of rounds
        //
        public void run()
        {
            for(int i = 0; i < 50; i++)
            {
                step(i);
            }
            
        }

        //
        // step member function steps through movement and hunting checks for
        // each morg in simulators morg list
        //   
        public void step(int seed)
        {
            
            foreach( Morg element in morgList)
            {
                //
                //Breaks loop if current morg is no longer living
                //
                if (!element.getLiving())
                    break;

                int[] elementLocation = element.getLocation();

                //
                //Hunting movements
                //
                if (element.getFollowstatus())
                {
                    Morg prey = element.getPrey();

                    int moveX = 0;
                    int moveY = 0;

                    int newLocX;
                    int newLocY;

                    int[] preylocation = prey.getLocation();

                    if (preylocation[0] < elementLocation[0])
                    {
                        moveX = -1;
                    }
                    else if(preylocation[0] > elementLocation[0])
                    {
                        moveX = 1;
                    }
                    else if(preylocation[0] == elementLocation[0])
                    {
                        moveX = 0;
                    }

                    if (preylocation[1] < elementLocation[1])
                    {
                        moveY = -1;
                    }
                    else if (preylocation[1] > elementLocation[1])
                    {
                        moveY = 1;
                    }
                    else if (preylocation[1] == elementLocation[1])
                    {
                        moveY = 0;
                    }

                    element.Move(elementLocation[0] + moveX, elementLocation[1] + moveY);

                    //
                    // If hunter encounters same square as prey
                    //
                    if(element.getLocation()[0] == prey.getLocation()[0] && element.getLocation()[1] == prey.getLocation()[1])
                    {
                        element.feed(prey);

                        prey.UpdateLiving();

                        element.setPrey(null);

                        
                    }

                }

                //
                //Normal movement behaviors for morg when not hunting
                //
                else
                {
                    Random rnd = new Random(seed);
                    int moveX = rnd.Next(-1, 1);
                    int moveY = rnd.Next(-1, 1);

                    element.Move(elementLocation[0] + moveX, elementLocation[1] + moveY);
                    


                    foreach (Morg preyLook in morgList)
                    {

                        if (toHunt(element, preyLook))
                        {
                            int preyX = preyLook.getLocation()[0];

                            int preyY = preyLook.getLocation()[1];

                            int rangeX = elementLocation[0] - preyX;

                            int rangeY = elementLocation[1] - preyY;

                            if (rangeY <= 3 || rangeY >= -3 || rangeX <= 3 || rangeY >= 3)
                            {
                                element.setPrey(preyLook);                             
                            }

                       }
                    }
                }

                
            }
        }

        //
        // Check function to see if Morg should hunt potential prey.
        //
        public bool toHunt(Morg hunter, Morg prey)
        {
            if (hunter.getMorgtype() == 'A' && (prey.getMorgtype() == 'B' || prey.getMorgtype() == 'C') && prey.getLiving())
                return true;
            if (hunter.getMorgtype() == 'B' && prey.getMorgtype() == 'A' && prey.getLiving())
                return true;
            if (hunter.getMorgtype() == 'C' && (prey.getMorgtype() == 'A' || prey.getMorgtype() == 'B') &&  prey.getLiving())
                return true;
            else
                return false;
        }

       
    }

    
}

