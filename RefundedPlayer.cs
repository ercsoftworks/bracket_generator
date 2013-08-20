using System;

namespace Brackets2012
{
    public class RefundedPlayer
    {
        private String _name;
        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        private string _refundAmount;
        public string RefundAmount
        {
            get
            {
                return this._refundAmount;
            }
            set
            {
                this._refundAmount = value;
            }
        }

        public double rawRefund;

        public RefundedPlayer(Player p)
        {
            Name = p.wholeName;
            RefundAmount = "$" + (p.refundOwed.ToString());
            rawRefund = p.refundOwed;
        }
    }
}
