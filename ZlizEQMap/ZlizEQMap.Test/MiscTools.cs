using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ZlizEQMap.Test
{
	[TestClass]
	public class MiscTools
	{
		[TestMethod]
		public void RenameImageFiles()
		{
			foreach (ZoneData zone in ZoneDataFactory.GetAllZones("The Al'Kabor Project"))
			{
				string zoneMapsPath = @"E:\Alexander\Software Projects\ZlizEQMap\ZlizEQMap\ZoneMaps";

				string imagePath = Path.Combine(Path.Combine(zoneMapsPath, zone.ContinentSortOrderString + zone.Continent), String.Format("{0}{1}.jpg", zone.FullName, zone.SubMapIndex.ToString()));
				string newPath = Path.Combine(Path.Combine(zoneMapsPath, zone.ContinentSortOrderString + zone.Continent), String.Format("{0}{1}.jpg", zone.ShortName, zone.SubMapIndex.ToString()));

				File.Move(imagePath, newPath);
			}
		}

		[TestMethod]
		public void FixNewLines()
		{
			foreach (string file in Directory.GetFiles(@"E:\Alexander\Software Projects\ZlizEQMap\ZlizEQMap\ZoneData", "*.txt", SearchOption.AllDirectories))
			{
				if (!Path.GetFileName(file).StartsWith("Zone"))
				{
					string text = File.ReadAllText(file);
					text = text.TrimEnd('\r', '\n');

					File.WriteAllText(file, text);
				}
			}
		}

		[TestMethod]
		public void FixZoneConnectionsInZoneDataFiles()
		{
			foreach (string file in Directory.GetFiles(@"E:\Alexander\Software Projects\ZlizEQMap\ZlizEQMap\ZoneData", "*.txt", SearchOption.AllDirectories))
			{
				if (!Path.GetFileName(file).StartsWith("Zone"))
				{
					List<string> lines = new List<string>();
					lines.AddRange(File.ReadAllLines(file));

					string line1 = lines[0];
					line1 = line1.Substring(0, line1.LastIndexOf('|'));

					lines[0] = line1;

					File.WriteAllLines(file, lines);
				}
			}
		}

		[TestMethod]
		public void RenameZoneDataFiles()
		{
			Dictionary<string, string> zones = new Dictionary<string, string>();

			foreach (string line in File.ReadAllLines(@"E:\Alexander\Software Projects\ZlizEQMap\ZlizEQMap\ZoneData\ZoneData-The Al'Kabor Project.txt"))
			{
				if (!String.IsNullOrEmpty(line) && !line.StartsWith("//"))
					zones.Add(line.Substring(line.IndexOf('=') + 1), line.Substring(0, line.IndexOf('=')));
			}

			foreach (string file in Directory.GetFiles(@"E:\Alexander\Software Projects\ZlizEQMap\ZlizEQMap\ZoneData", "*.txt", SearchOption.AllDirectories))
			{
				if (!Path.GetFileName(file).StartsWith("Zone"))
				{
					string filename = Path.GetFileNameWithoutExtension(file);
					string zoneName = filename;
					zoneName = zoneName.Substring(0, zoneName.Length - 1);
					string shortZoneName = zones[zoneName];

					string fileName = Path.GetFileName(file);
					string dir = Path.GetDirectoryName(file);

					string newFileName = shortZoneName + filename.Substring(filename.Length - 1) + ".txt";
					File.Move(file, Path.Combine(dir, newFileName));
				}
			}
		}

		[TestMethod]
		public void GenerateZoneConnectionsFile()
		{
			Dictionary<string, string> zones = new Dictionary<string, string>();

			foreach (string line in File.ReadAllLines(@"E:\Alexander\Software Projects\ZlizEQMap\ZlizEQMap\ZoneData\ZoneData-Project1999.txt"))
			{
				if (!String.IsNullOrEmpty(line) && !line.StartsWith("//"))
					zones.Add(line.Substring(line.IndexOf('=') + 1), line.Substring(0, line.IndexOf('=')));
			}

			List<string> zonesTemp = new List<string>();

			foreach (string file in Directory.GetFiles(@"E:\Alexander\Software Projects\ZlizEQMap\ZlizEQMap\ZoneData", "*.txt", SearchOption.AllDirectories))
			{
				if (!Path.GetFileName(file).StartsWith("Zone"))
				{
					string zoneName = Path.GetFileNameWithoutExtension(file);
					zoneName = zoneName.Substring(0, zoneName.Length - 1);

					if (!zonesTemp.Contains(zoneName))
					{
						string shortZoneName = zones[zoneName];

						string line1 = File.ReadAllLines(file)[0];
						string connectionsString = line1.Substring(line1.LastIndexOf('|') + 1);
						string connectionsStringsShort = "";

						List<string> connsList = new List<string>();

						foreach (string conn in connectionsString.Split(','))
						{
							if (!String.IsNullOrEmpty(conn.Trim()))
							{
								connsList.Add(zones[conn.Trim()]);
							}
						}

						connsList.Sort();

						foreach (string con in connsList)
						{
							connectionsStringsShort += con + ',';
						}

						connectionsStringsShort = connectionsStringsShort.TrimEnd(',');

						using (TextWriter tw = new StreamWriter(@"E:\Alexander\Downloads\zoneconns.txt", true))
						{
							tw.WriteLine(String.Format("{0}={1}", shortZoneName, connectionsStringsShort));
						}


						zonesTemp.Add(zoneName);
					}
				}
			}
		}

		[TestMethod]
		public void GenerateZoneDataFile()
		{
			List<string> zones = new List<string>();
			List<string> conts = new List<string>();

			foreach (string file in Directory.GetFiles(@"E:\Alexander\Software Projects\ZlizEQMap\ZlizEQMap\ZoneData", "*.txt", SearchOption.AllDirectories))
			{
				if (!Path.GetFileName(file).StartsWith("Zone"))
				{
					string continent = Path.GetDirectoryName(file);
					continent = continent.Substring(continent.LastIndexOf('\\') + 1);

					if (!conts.Contains(continent))
					{
						using (TextWriter tw = new StreamWriter(@"E:\Alexander\Downloads\zonedata.txt", true))
						{
							tw.WriteLine(String.Format("// {0}", continent));
						}

						conts.Add(continent);
					}

					string zoneName = Path.GetFileNameWithoutExtension(file);
					zoneName = zoneName.Substring(0, zoneName.Length - 1);

					if (!zones.Contains(zoneName))
					{
						using (TextWriter tw = new StreamWriter(@"E:\Alexander\Downloads\zonedata.txt", true))
						{
							tw.WriteLine(String.Format("{0}={0}", zoneName));
						}

						zones.Add(zoneName);
					}
				}
			}
		}

		[TestMethod]
		public void FixMapFiles()
		{
			foreach (string file in Directory.GetFiles(@"E:\Alexander\Software Projects\ZlizEQMap\ZlizEQMap\ZoneData", "*.txt", SearchOption.AllDirectories))
			{
				string zoneName = Path.GetFileNameWithoutExtension(file);
				zoneName = zoneName.Substring(0, zoneName.Length - 1);

				string fileData = File.ReadAllText(file);

				fileData = zoneName + "|" + fileData;

				File.WriteAllText(file, fileData);
			}
		}
	}
}
