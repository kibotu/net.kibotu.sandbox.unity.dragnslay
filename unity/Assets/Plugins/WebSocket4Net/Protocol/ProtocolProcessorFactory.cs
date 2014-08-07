using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WebSocket4Net.Protocol
{
    class ProtocolProcessorFactory
    {
        private IProtocolProcessor[] m_OrderedProcessors;

        public ProtocolProcessorFactory(params IProtocolProcessor[] processors)
        {
            m_OrderedProcessors = LINQ.ToArray(LINQ.OrderByDescending(processors, p => (int)p.Version));
        }

        public IProtocolProcessor GetProcessorByVersion(WebSocketVersion version)
        {
            return LINQ.FirstOrDefault(m_OrderedProcessors,p => p.Version == version);
        }

        public IProtocolProcessor GetPreferedProcessorFromAvialable(int[] versions)
        {
            foreach (var v in LINQ.OrderByDescending(versions,i => i))
            {
                foreach (var n in m_OrderedProcessors)
                {
                    int versionValue = (int)n.Version;

                    if (versionValue < v)
                        break;

                    if (versionValue > v)
                        continue;

                    return n;
                }
            }

            return null;
        }
    }
}
