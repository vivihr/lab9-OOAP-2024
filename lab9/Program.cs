using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9
{

    //This class represents the vending machine itself.
    //It holds the current state of the vending machine and provides methods to interact with it.
    class VendingMachine
    {
        private VendingMachineState currentState;
        public int ItemPrice { get; }
        public int CustomerMoney { get; private set; }

        public VendingMachine(int itemPrice)
        {
            ItemPrice = itemPrice;
            currentState = new ReadyState(this);
        }

        public void InsertMoney(int money)
        {
            CustomerMoney += money;
            Console.WriteLine($"Inserted {money} money. Total: {CustomerMoney}");
            currentState.Handle();
        }

        public void SelectItem()
        {
            currentState = new ItemSelectedState(this);
            currentState.Handle();
        }

        public void ConfirmPurchase()
        {
            currentState = new ConfirmPurchaseState(this);
            currentState.Handle();
        }

        public void CancelPurchase()
        {
            currentState = new ReadyState(this);
            Console.WriteLine("Purchase canceled.");
        }

        public void DispenseItem()
        {
            Console.WriteLine("Item have been issued. Thank you for your purchase!");
        }

        public void DispenseChange()
        {
            int change = CustomerMoney - ItemPrice;
            if (change > 0)
                Console.WriteLine($"Change of {change} dispensed.");
        }

        public void SetState(VendingMachineState state)
        {
            currentState = state;
        }
    }

    //This abstract class defines the interface for handling the state transitions of the vending machine.
    abstract class VendingMachineState
    {
        protected VendingMachine vendingMachine;

        public VendingMachineState(VendingMachine vendingMachine)
        {
            this.vendingMachine = vendingMachine;
        }

        public abstract void Handle();
    }

    //This state represents the machine being ready and waiting for customer interaction.
    class ReadyState : VendingMachineState
    {
        public ReadyState(VendingMachine vendingMachine) : base(vendingMachine) { }

        public override void Handle()
        {
            Console.WriteLine("Waiting for customer...");
        }
    }

    //This state represents the machine having an item selected by the customer.
    class ItemSelectedState : VendingMachineState
    {
        public ItemSelectedState(VendingMachine vendingMachine) : base(vendingMachine) { }

        public override void Handle()
        {
            Console.WriteLine("Item selected. Please confirm your purchase.");
        }
    }

    //This state represents the machine after the customer has confirmed the purchase.
    class ConfirmPurchaseState : VendingMachineState
    {
        public ConfirmPurchaseState(VendingMachine vendingMachine) : base(vendingMachine) { }

        public override void Handle()
        {
            if (vendingMachine.CustomerMoney >= vendingMachine.ItemPrice)
            {
                if (vendingMachine.CustomerMoney > vendingMachine.ItemPrice)
                {
                    vendingMachine.DispenseItem();
                    vendingMachine.DispenseChange();
                }
                else
                {
                    vendingMachine.DispenseItem();
                    Console.WriteLine("Exact amount received. No change to dispense.");
                }
                vendingMachine.SetState(new ReadyState(vendingMachine));
            }
            else
            {
                Console.WriteLine("Insufficient funds. Please insert more money or cancel the purchase.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //creating vending machine with item price of 10
            VendingMachine vendingMachine = new VendingMachine(10);

            //simulating customer actions
            Console.WriteLine("Simulating customer actions...");

            vendingMachine.InsertMoney(5); //inserted 5 money so total price is 5
            vendingMachine.InsertMoney(10); //inserted 10 money so total price is 15
            vendingMachine.SelectItem(); //item selected. Please confirm your purchase.
            vendingMachine.ConfirmPurchase(); //item issued. Thank you for your purchase! Change of 5 dispensed.

            Console.WriteLine("\nSimulating another customer action...");

            VendingMachine vendingMachine2 = new VendingMachine(10);

            vendingMachine2.InsertMoney(5); //inserted 5 money so total price is 5
            vendingMachine2.SelectItem(); //item selected. Please confirm your purchase.
            vendingMachine2.ConfirmPurchase(); //insufficient funds. Please insert more money or cancel the purchase.

            Console.WriteLine("\nSimulating another customer action with exact amount...");

            VendingMachine vendingMachine3 = new VendingMachine(10);

            vendingMachine3.InsertMoney(10); //inserted 10 moneyso total = 10
            vendingMachine3.SelectItem(); //item selected. Please confirm your purchase.
            vendingMachine3.ConfirmPurchase(); //item is issued. Thank you for your purchase! Exact amount received. No change to dispense.

            Console.ReadLine();
        }
    }
}