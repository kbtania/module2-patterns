using System;

namespace ConsoleApp1
{
    class Card
    {
        private CreditState _state;
        public double balance;
        public double credit_limit;
        public DateTime grace_period;
        public int percent;
        public string card_num;
        public double credit_return;
      

        public Card(CreditState state, double balance, double credit_limit, DateTime grace_period, int percent, string card_num)
        {
            this._state = state;
            this.balance = balance;
            this.credit_limit = credit_limit;
            this.grace_period = grace_period;
            this.percent = percent;
            this.card_num = card_num;
        }

        

        public void AddMoney(double sum)
        {
            if (this.credit_return != 0)
            {
                this.credit_limit += (sum-this.credit_return);
            }
            else
            {
                this.balance += sum;
            }
        }
        public void SpendMoney(double sum)
        {
            if (this.balance >= sum)
            {
                this.balance -= sum;
            } 
            else
            {
                double take_from_credit = sum - this.balance;
                this.balance -= sum - this.balance;
                this.credit_return += take_from_credit;
                this.credit_limit -= take_from_credit;
            }
        }
        public void Percent()
        {
            var today = DateTime.Now;
            int result = DateTime.Compare(this.grace_period, today);
            if (result < 0)
            {
                _state = new AddPercent(this);
                _state.Handle();
            }
            else
            {
                _state = new NoPercent(this);
                _state.Handle();
            }
        }
        public override string ToString()
        {
            return $"Balance: {this.balance}\nCredit debt: {this.credit_return}\nCredit limit: {this.credit_limit}";
        }
       
    }


    abstract class CreditState
    {
        public Card card;

        public void SetContext(Card card)
        {
            this.card = card;
        }

        public abstract void Handle();
    }


    class AddPercent : CreditState  // Якщо пільговий період вже завершився (сьогодні-кінець пільгового періоду > 0)
    {
        public AddPercent(Card card)
        {
            this.card = card;
        }
        public override void Handle()
        {
            this.card.credit_return += card.credit_return * this.card.percent;
            Console.WriteLine($"Credit debt: {this.card.credit_return}");
        }    
    }

    class NoPercent : CreditState // Якщо пільговий період ще триває
    {
        public NoPercent(Card card)
        {
            this.card = card;

        }
        public override void Handle()
        {
            Console.WriteLine($"Credit debt: 0");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var c1 = new Card(null, 0, 1000, new DateTime(2022, 1, 1), 10, "123 456 7890");
            Console.WriteLine("Before");
            Console.WriteLine(c1);
            c1.AddMoney(300);
            Console.WriteLine("After adding 300$");
            Console.WriteLine(c1);
            c1.SpendMoney(400);
            Console.WriteLine(c1);
            c1.Percent();
        }

    }
}
