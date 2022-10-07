using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BillOnPay
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bill bill = new Bill(1000,1,0);
            PartBill partbill = new PartBill(bill._billOnDay, bill._dayCount, bill._penalty, bill._dayPenaltyCount);
            
            AllBills ab = new AllBills();
            Bill bill2 = new Bill(2000,5,4);
            ab.Add(bill2);
            ab.Add(bill2);
            ab.Add(bill2);

            Bill.FullPayment = false;
            using (FileStream fs = new FileStream("bill.xml", FileMode.Create)) { }
            using (FileStream fs = new FileStream("allbill.xml", FileMode.Create)) { }
            using (FileStream fs = new FileStream("bill.xml", FileMode.Truncate))
            {
                if (Bill.FullPayment)
                {
                    XmlSerializer xseria = new XmlSerializer(typeof(Bill));
                    xseria.Serialize(fs, bill);
                    bill.Display();
                }
                else
                {
                    XmlSerializer xseria = new XmlSerializer(typeof(PartBill));
                    xseria.Serialize(fs, partbill);
                    partbill.Display();
                }
            }
            using (FileStream fs = new FileStream("bill.xml", FileMode.Open))
            {
                if (Bill.FullPayment)
                {
                    XmlSerializer xseria = new XmlSerializer(typeof(Bill));
                    bill = xseria.Deserialize(fs) as Bill;
                    bill.Display();
                }
                else
                {
                    XmlSerializer xseria = new XmlSerializer(typeof(PartBill));
                    partbill = xseria.Deserialize(fs) as PartBill;
                    partbill.Display();
                }
            }
            //Console.ReadLine();
            using (FileStream fs = new FileStream("allbill.xml", FileMode.Truncate))
            {
                    XmlSerializer xseria = new XmlSerializer(typeof(AllBills));
                    xseria.Serialize(fs, ab);
            }
            using (FileStream fs = new FileStream("allbill.xml", FileMode.Open))
            {                
                XmlSerializer xseria = new XmlSerializer(typeof(AllBills));
                AllBills result = xseria.Deserialize(fs) as AllBills;
                foreach (Bill b in result.bills)
                {
                    b.Display();
                }
               
            }
            
        }
    }

    public class AllBills 
    {

        public List<Bill> bills = new List<Bill>();

        public void Add(Bill obj)
        {
            bills.Add(obj);
        }

        public AllBills() { }

    } 
    public class PartBill 
    {
        public decimal _billOnDay;
        public int _dayCount;
        public int _penalty = 1;
        public int _dayPenaltyCount;

        public PartBill() { }

        public PartBill(decimal BillOnDay, int DayCount, int Penalty, int DayPenaltyCount) 
        {
            _billOnDay = BillOnDay;
            _dayCount = DayCount;
            _penalty = Penalty;
            _dayPenaltyCount = DayPenaltyCount;
        }
        public void Display()
        {
            Console.WriteLine($"Объект: {_billOnDay} {_dayCount} {_dayPenaltyCount}");
        }
    }
    public class Bill 
    {
        public decimal _billOnDay;
        public int _dayCount;
        public int _penalty = 1;
        public int _dayPenaltyCount;
        public decimal _paymentWithoutPenalty;
        public decimal _paymentPenalty;
        public decimal _commonPayment;

        public static bool FullPayment { get; set; } = true;

        public Bill() { }
        public Bill(decimal BillOnDay, int DayCount, int DayPenaltyCount) 
        {
            _billOnDay = BillOnDay;
            _dayCount = DayCount;
            _dayPenaltyCount = DayPenaltyCount;
            _paymentWithoutPenalty = BillOnDay * DayCount;
            _paymentPenalty = _paymentWithoutPenalty * _penalty / 100;
            _commonPayment = _paymentWithoutPenalty + _paymentPenalty;
        }

        public void Display() 
        {
            Console.WriteLine($"Объект: {_billOnDay} {_dayCount} {_dayPenaltyCount} {_paymentWithoutPenalty} {_paymentPenalty} {_commonPayment}");
        }
    }
}