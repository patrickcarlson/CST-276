using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    class Program
    {
        static void Main(string[] args)
        {
            A Morg1 = new A();

            Morg1.setLiving();

            Console.WriteLine("Morg1 is ");
            if ( Morg1.getLiving() )
                Console.WriteLine("alive!");
            else
                Console.WriteLine("dead!");
        }
    }

    interface IMorgObservers
    {
        void UpdateLocation();
        void UpdateLiving();
    }

    abstract class Morg
    {
        private bool living;
        private int[] location;
        private int direction; //type?
        private List<IMorgObservers> observers;
        private Morg prey;
        public Morg(/*double cLocation, int cDirection*/)
        {
            living = true;
           // location = cLocation;
            //direction = cDirection;
        }
        public bool getLiving() { return living;  }

        public void setLiving()
        {
            if (living == true)
                living = false;
            else
                living = true;
        }

        public void UpdateLocation()
        {

        }

        public void UpdateLiveness()
        {

        }

        public void Move(int[] newLocation)
        {

        }

        public void registerObserver(IMorgObservers o)
        {

        }
    }

    class A : Morg
    {

    }

    class B : Morg
    {

    }

    class C : Morg
    {

    }
    class Simulator 
    {

    }

    class Dish
    {

    }
}
