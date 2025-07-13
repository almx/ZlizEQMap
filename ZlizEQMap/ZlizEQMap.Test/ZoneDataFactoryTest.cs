using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZlizEQMap.Test
{
	[TestClass]
	public class ZoneDataFactoryTest
	{
		[TestMethod]
		public void GetZoneConnectionsTest()
		{
			Dictionary<string, List<string>> connections = ZoneDataFactory.GetZoneConnections();
		}

		[TestMethod]
		public void GetZoneDataTest()
		{
			Dictionary<string, string> zoneData = ZoneDataFactory.GetZoneData("Project1999");
		}

		[TestMethod]
		public void GetAllZonesTest()
		{
			List<ZoneData> zones = ZoneDataFactory.GetAllZones("Project1999");
		}
	}
}
