using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RIVM
{
    public class IOPort
    {
        private Func<int> _get;
        private Action<int> _set;

        public int Register
        {
            get
            {
                return _get();
            }
            set
            {
                _set(value);
            }
        }

        public IOPort(Func<int> get, Action<int> set)
        {
            _get = get;
            _set = set;
        }
    }
}
