using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using LibObjectFile.Elf;
using LibObjectFile.Dwarf;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;

namespace UsbDeviceBaseTest
{
    public class ELFReaderTest : XUnitTestBase
    {

        public ELFReaderTest() : base()
        {

        }

        [Fact]
        public void TestElfFile()
        {
            //// Reads an ELF file
            var resourceName = @"D:\ALAIN\AT90USB\Projects\AT90USB\Debug\AT90USB.elf";

            Logger.Info($"INFO: Testing ELF file: {resourceName}");

            using FileStream inStream = File.OpenRead(resourceName);
            ElfFile? elf = ElfFile.Read(inStream);
            Assert.NotNull(elf);

            StringBuilder sb = new StringBuilder();
            sb.Append($"ELF Header:");
            sb.Append($"  Type: {elf.FileType}");
            sb.Append($"  Version: {elf.Version}");
            sb.Append($"  Class: {elf.FileClass}");
            sb.Append($"  Encoding: {elf.Encoding}");
            sb.AppendLine();
            Logger.Info(sb.ToString());

            Logger.Info($"ElfSegment\r");
            foreach (ElfSegment segment in elf.Segments)
            {
                Logger.Info($"TypeOfSegment : {segment.GetType()} Range: {segment.Range}, VirtualAddress: 0x{segment.VirtualAddress:X}, PhysicalAddress: 0x{segment.PhysicalAddress:X}, Type: {segment.Type}, Flags: {segment.Flags}, Size: {segment.SizeInMemory} bytes, AdditionalDataSize: {segment.AdditionalData.Length}");
            }

            Logger.Info($"ElfSection\r");
            foreach (ElfSection section in elf.Sections)
            {
                Logger.Info($"TypeOfSection : {section.GetType()} Name: {section.Name.Value}, Index: 0x{section.Index:X}, Type: {section.Type}, Flags: {section.Flags}, Size: {section.Size} bytes");
            }

            Logger.Info($"ElfSymbolTable\r");
            var symbolTable = elf.Sections.FirstOrDefault(e => e.Name.Value == ".symtab");

            if (symbolTable != null)
                foreach (ElfSymbol f in ((ElfSymbolTable)symbolTable).Entries)
                    Logger.Info($"Name: {f.Name.Value}, Value: 0x{f.Value:X}, Size: {f.Size}, Type: {f.Type}, Binding: {f.Bind}, Visibility: {f.Visibility}");

            Logger.Info($"StringTable\r");
            ElfStringTable? stringTable = (ElfStringTable?)elf.Sections.FirstOrDefault(e => e.Name.Value == ".strtab");

            if (stringTable != null && stringTable.HasContent)
            {
                Logger.Info($"Name: {stringTable.Name.Value}, Index: {stringTable.Index}, TableEntrySize: {stringTable.TableEntrySize}, Size: {stringTable.Size}, Flags:{stringTable.Flags}");
                ElfString l = "main";
                if (stringTable.TryResolve(l, out ElfString entry))
                {
                    Logger.Info($"{l.Value} -> String: {entry.Value}, index: 0x{entry.Index:X}");
                }
            }
            else
            {
                Logger.Info($"Name: {stringTable?.Name.Value}, Empty.");
            }

            Logger.Info($"DWARF\r");

            using var inStream2 = File.OpenRead(resourceName);
            ElfFile? elf2 = ElfFile.Read(inStream2);
            Assert.NotNull(elf2);

            DwarfElfContext elfContext = new DwarfElfContext(elf2);
            DwarfReaderContext inputContext = new DwarfReaderContext(elfContext);
            inputContext.DebugLinePrinter = Console.Out;
            DwarfFile dwarf = DwarfFile.Read(inputContext);
            Assert.NotNull(dwarf);
            return;
        }
    }
}


