using iTextSharp.text;
using iTextSharp.text.pdf;
using laundrycucikilat.Models;
using System.Text;

namespace laundrycucikilat.Services
{
    public class PdfService
    {
        public byte[] GenerateOrderDetailPdf(Order order)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Create document
                var document = new Document(PageSize.A4, 40, 40, 40, 40);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                
                document.Open();

                // Register fonts for Indonesian text
                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                var titleFont = new Font(baseFont, 18, Font.BOLD, new BaseColor(0, 0, 255));
                var headerFont = new Font(baseFont, 14, Font.BOLD, new BaseColor(0, 0, 0));
                var normalFont = new Font(baseFont, 10, Font.NORMAL, new BaseColor(0, 0, 0));
                var boldFont = new Font(baseFont, 10, Font.BOLD, new BaseColor(0, 0, 0));

                // Header with logo and company info
                AddHeader(document, titleFont, normalFont);

                // Add spacing
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                // Title
                var title = new Paragraph("DETAIL PESANAN LAUNDRY", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                document.Add(new Paragraph(" "));

                // Order Information Section
                AddOrderInformation(document, order, headerFont, boldFont, normalFont);

                document.Add(new Paragraph(" "));

                // Customer Information Section
                AddCustomerInformation(document, order, headerFont, boldFont, normalFont);

                document.Add(new Paragraph(" "));

                // Service Details Section
                AddServiceDetails(document, order, headerFont, boldFont, normalFont);

                document.Add(new Paragraph(" "));

                // Footer
                AddFooter(document, normalFont);

                document.Close();
                return memoryStream.ToArray();
            }
        }

        private void AddHeader(Document document, Font titleFont, Font normalFont)
        {
            var headerTable = new PdfPTable(2);
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 1, 2 });

            // Logo placeholder (you can add actual logo here)
            var logoCell = new PdfPCell(new Phrase("🧺", new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 24)));
            logoCell.Border = Rectangle.NO_BORDER;
            logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
            logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerTable.AddCell(logoCell);

            // Company info
            var companyInfo = new PdfPCell();
            companyInfo.Border = Rectangle.NO_BORDER;
            companyInfo.AddElement(new Paragraph("LAUNDRY CUCI KILAT", titleFont));
            companyInfo.AddElement(new Paragraph("Layanan Laundry Terpercaya", normalFont));
            companyInfo.AddElement(new Paragraph("Jl. Contoh No. 123, Jakarta", normalFont));
            companyInfo.AddElement(new Paragraph("Telp: (021) 1234-5678", normalFont));
            companyInfo.AddElement(new Paragraph("Email: info@laundrycucikilat.com", normalFont));
            headerTable.AddCell(companyInfo);

            document.Add(headerTable);

            // Add line separator
            var line = new Paragraph("_".PadRight(80, '_'), normalFont);
            line.Alignment = Element.ALIGN_CENTER;
            document.Add(line);
        }

        private void AddOrderInformation(Document document, Order order, Font headerFont, Font boldFont, Font normalFont)
        {
            var orderHeader = new Paragraph("INFORMASI PESANAN", headerFont);
            orderHeader.SpacingAfter = 10;
            document.Add(orderHeader);

            var orderTable = new PdfPTable(4);
            orderTable.WidthPercentage = 100;
            orderTable.SetWidths(new float[] { 1, 1, 1, 1 });

            // Row 1
            AddTableCell(orderTable, "ID Pesanan:", boldFont);
            AddTableCell(orderTable, order.OrderId ?? "N/A", normalFont);
            AddTableCell(orderTable, "Tanggal Pesan:", boldFont);
            AddTableCell(orderTable, FormatDate(order.TanggalPesan), normalFont);

            // Row 2
            AddTableCell(orderTable, "Status:", boldFont);
            AddTableCell(orderTable, order.Status ?? "N/A", normalFont);
            AddTableCell(orderTable, "Estimasi Selesai:", boldFont);
            AddTableCell(orderTable, order.EstimasiSelesai ?? "N/A", normalFont);

            // Row 3
            AddTableCell(orderTable, "Tanggal Ambil:", boldFont);
            AddTableCell(orderTable, order.TanggalAmbil ?? "N/A", normalFont);
            AddTableCell(orderTable, "Waktu Ambil:", boldFont);
            AddTableCell(orderTable, order.WaktuAmbil ?? "N/A", normalFont);

            document.Add(orderTable);
        }

        private void AddCustomerInformation(Document document, Order order, Font headerFont, Font boldFont, Font normalFont)
        {
            var customerHeader = new Paragraph("INFORMASI PELANGGAN", headerFont);
            customerHeader.SpacingAfter = 10;
            document.Add(customerHeader);

            var customerTable = new PdfPTable(2);
            customerTable.WidthPercentage = 100;
            customerTable.SetWidths(new float[] { 1, 2 });

            AddTableCell(customerTable, "Nama Lengkap:", boldFont);
            AddTableCell(customerTable, order.NamaLengkap ?? "N/A", normalFont);

            AddTableCell(customerTable, "No. Telepon:", boldFont);
            AddTableCell(customerTable, order.NoTelepon ?? "N/A", normalFont);

            AddTableCell(customerTable, "Alamat:", boldFont);
            AddTableCell(customerTable, order.Alamat ?? "N/A", normalFont);

            document.Add(customerTable);
        }

        private void AddServiceDetails(Document document, Order order, Font headerFont, Font boldFont, Font normalFont)
        {
            var serviceHeader = new Paragraph("DETAIL LAYANAN", headerFont);
            serviceHeader.SpacingAfter = 10;
            document.Add(serviceHeader);

            var serviceTable = new PdfPTable(2);
            serviceTable.WidthPercentage = 100;
            serviceTable.SetWidths(new float[] { 1, 2 });

            AddTableCell(serviceTable, "Jenis Layanan:", boldFont);
            AddTableCell(serviceTable, order.JenisLayanan ?? "N/A", normalFont);

            AddTableCell(serviceTable, "Berat Laundry:", boldFont);
            AddTableCell(serviceTable, $"{order.Jumlah} kg", normalFont);

            AddTableCell(serviceTable, "Antar Jemput:", boldFont);
            AddTableCell(serviceTable, order.AntarJemput ? "Ya" : "Tidak", normalFont);

            AddTableCell(serviceTable, "Pewangi Khusus:", boldFont);
            AddTableCell(serviceTable, order.PewangiKhusus ? "Ya" : "Tidak", normalFont);

            AddTableCell(serviceTable, "Status Pembayaran:", boldFont);
            AddTableCell(serviceTable, order.PaymentStatus ?? "Belum Lunas", normalFont);

            AddTableCell(serviceTable, "Metode Pembayaran:", boldFont);
            AddTableCell(serviceTable, order.PaymentMethod ?? "N/A", normalFont);

            document.Add(serviceTable);

            // Total amount in a highlighted box
            document.Add(new Paragraph(" "));
            var totalTable = new PdfPTable(1);
            totalTable.WidthPercentage = 50;
            totalTable.HorizontalAlignment = Element.ALIGN_RIGHT;

            var totalCell = new PdfPCell(new Phrase($"TOTAL BIAYA: {order.Total ?? "Rp 0"}", 
                new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 14, Font.BOLD, new BaseColor(255, 255, 255))));
            totalCell.BackgroundColor = new BaseColor(0, 0, 255);
            totalCell.HorizontalAlignment = Element.ALIGN_CENTER;
            totalCell.Padding = 10;
            totalTable.AddCell(totalCell);

            document.Add(totalTable);

            // Add notes if available
            if (!string.IsNullOrEmpty(order.Catatan))
            {
                document.Add(new Paragraph(" "));
                var notesHeader = new Paragraph("CATATAN KHUSUS:", headerFont);
                document.Add(notesHeader);
                var notes = new Paragraph(order.Catatan, normalFont);
                notes.SpacingAfter = 10;
                document.Add(notes);
            }
        }

        private void AddFooter(Document document, Font normalFont)
        {
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            var footerTable = new PdfPTable(2);
            footerTable.WidthPercentage = 100;
            footerTable.SetWidths(new float[] { 1, 1 });

            // Left side - print info
            var printInfo = new PdfPCell();
            printInfo.Border = Rectangle.NO_BORDER;
            printInfo.AddElement(new Paragraph($"Dicetak pada: {DateTime.Now:dd/MM/yyyy HH:mm}", normalFont));
            printInfo.AddElement(new Paragraph("Dokumen ini dicetak dari sistem Laundry Cuci Kilat", normalFont));
            footerTable.AddCell(printInfo);

            // Right side - signature area
            var signatureArea = new PdfPCell();
            signatureArea.Border = Rectangle.NO_BORDER;
            signatureArea.HorizontalAlignment = Element.ALIGN_CENTER;
            signatureArea.AddElement(new Paragraph("Petugas Laundry", normalFont));
            signatureArea.AddElement(new Paragraph(" ", normalFont));
            signatureArea.AddElement(new Paragraph(" ", normalFont));
            signatureArea.AddElement(new Paragraph("(_________________)", normalFont));
            footerTable.AddCell(signatureArea);

            document.Add(footerTable);

            // Add line separator
            var line = new Paragraph("_".PadRight(80, '_'), normalFont);
            line.Alignment = Element.ALIGN_CENTER;
            document.Add(line);

            // Footer text
            var footer = new Paragraph("Terima kasih telah menggunakan layanan Laundry Cuci Kilat", normalFont);
            footer.Alignment = Element.ALIGN_CENTER;
            footer.SpacingBefore = 10;
            document.Add(footer);
        }

        private void AddTableCell(PdfPTable table, string text, Font font)
        {
            var cell = new PdfPCell(new Phrase(text, font));
            cell.Border = Rectangle.NO_BORDER;
            cell.Padding = 5;
            table.AddCell(cell);
        }

        private string FormatDate(string dateString)
        {
            if (DateTime.TryParse(dateString, out DateTime date))
            {
                return date.ToString("dd/MM/yyyy");
            }
            return dateString ?? "N/A";
        }

        private string FormatDateForDisplay(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return "N/A";

            // Try different date formats
            string[] formats = { 
                "yyyy-MM-dd", 
                "dd/MM/yyyy", 
                "MM/dd/yyyy", 
                "yyyy-MM-dd HH:mm:ss",
                "dd-MM-yyyy",
                "MM-dd-yyyy"
            };

            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    return date.ToString("dd/MM/yyyy");
                }
            }

            // Try general parsing
            if (DateTime.TryParse(dateString, out DateTime generalDate))
            {
                return generalDate.ToString("dd/MM/yyyy");
            }

            // If all fails, return the original string
            Console.WriteLine($"Failed to parse date: {dateString}");
            return dateString;
        }

        public byte[] GenerateTransactionReportPdf(List<Order> orders, string startDate, string endDate)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Create document in landscape orientation for better table display
                var document = new Document(PageSize.A4.Rotate(), 30, 30, 30, 30);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                
                document.Open();

                // Register fonts
                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                var titleFont = new Font(baseFont, 16, Font.BOLD, new BaseColor(0, 0, 255));
                var headerFont = new Font(baseFont, 12, Font.BOLD, new BaseColor(0, 0, 0));
                var normalFont = new Font(baseFont, 9, Font.NORMAL, new BaseColor(0, 0, 0));
                var boldFont = new Font(baseFont, 9, Font.BOLD, new BaseColor(0, 0, 0));
                var smallFont = new Font(baseFont, 8, Font.NORMAL, new BaseColor(0, 0, 0));

                // Debug logging
                Console.WriteLine($"PdfService: Generating PDF for {orders.Count} orders");
                Console.WriteLine($"PdfService: Date range {startDate} to {endDate}");

                // Header with company info
                AddReportHeader(document, titleFont, normalFont);

                // Add spacing
                document.Add(new Paragraph(" "));

                // Report title
                var title = new Paragraph("LAPORAN TRANSAKSI LAUNDRY", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Date range - improved formatting
                var formattedStartDate = FormatDateForDisplay(startDate);
                var formattedEndDate = FormatDateForDisplay(endDate);
                var dateRange = new Paragraph($"Periode: {formattedStartDate} - {formattedEndDate}", headerFont);
                dateRange.Alignment = Element.ALIGN_CENTER;
                document.Add(dateRange);

                document.Add(new Paragraph(" "));

                // Check if we have data
                if (orders == null || orders.Count == 0)
                {
                    // Add empty data message
                    var noDataMessage = new Paragraph("Tidak ada transaksi pada periode ini", headerFont);
                    noDataMessage.Alignment = Element.ALIGN_CENTER;
                    noDataMessage.SpacingBefore = 50;
                    noDataMessage.SpacingAfter = 50;
                    document.Add(noDataMessage);
                    
                    // Still add summary with zeros
                    AddReportSummary(document, new List<Order>(), headerFont, boldFont, normalFont);
                }
                else
                {
                    // Summary statistics
                    AddReportSummary(document, orders, headerFont, boldFont, normalFont);

                    document.Add(new Paragraph(" "));

                    // Transaction table
                    AddTransactionTable(document, orders, headerFont, normalFont, smallFont);
                }

                // Footer
                AddReportFooter(document, normalFont);

                document.Close();
                return memoryStream.ToArray();
            }
        }

        private void AddReportHeader(Document document, Font titleFont, Font normalFont)
        {
            // Company header
            var headerTable = new PdfPTable(2);
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 70, 30 });

            // Company info
            var companyInfo = new PdfPCell();
            companyInfo.Border = Rectangle.NO_BORDER;
            companyInfo.AddElement(new Paragraph("LAUNDRY CUCI KILAT", new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 14, Font.BOLD, new BaseColor(0, 0, 255))));
            companyInfo.AddElement(new Paragraph("Layanan Laundry Profesional", normalFont));
            companyInfo.AddElement(new Paragraph("Jl. Contoh No. 123, Jakarta", normalFont));
            companyInfo.AddElement(new Paragraph("Telp: (021) 1234-5678 | Email: info@laundrycucikilat.com", normalFont));
            headerTable.AddCell(companyInfo);

            // Logo placeholder
            var logoCell = new PdfPCell(new Phrase("LOGO", normalFont));
            logoCell.Border = Rectangle.BOX;
            logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
            logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            logoCell.FixedHeight = 60f;
            headerTable.AddCell(logoCell);

            document.Add(headerTable);

            // Separator line
            var line = new Paragraph("_".PadRight(120, '_'), normalFont);
            line.Alignment = Element.ALIGN_CENTER;
            document.Add(line);
        }

        private void AddReportSummary(Document document, List<Order> orders, Font headerFont, Font boldFont, Font normalFont)
        {
            var summaryTable = new PdfPTable(4);
            summaryTable.WidthPercentage = 100;
            summaryTable.SetWidths(new float[] { 25, 25, 25, 25 });

            // Calculate statistics with improved error handling
            var totalTransactions = orders.Count;
            var completedOrders = orders.Count(o => o.Status == "Selesai");
            var pendingOrders = orders.Count(o => o.Status != "Selesai");
            
            // Improved revenue calculation
            decimal totalRevenue = 0;
            foreach (var order in orders.Where(o => o.Status == "Selesai"))
            {
                if (!string.IsNullOrEmpty(order.Total))
                {
                    // Remove currency formatting and parse
                    var cleanTotal = order.Total
                        .Replace("Rp", "")
                        .Replace(".", "")
                        .Replace(",", "")
                        .Replace(" ", "")
                        .Trim();
                    
                    if (decimal.TryParse(cleanTotal, out decimal amount))
                    {
                        totalRevenue += amount;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to parse total: '{order.Total}' -> '{cleanTotal}'");
                    }
                }
            }

            Console.WriteLine($"PdfService Summary: Total={totalTransactions}, Completed={completedOrders}, Pending={pendingOrders}, Revenue={totalRevenue}");

            // Summary cells
            AddSummaryCell(summaryTable, "Total Transaksi", totalTransactions.ToString(), boldFont, normalFont);
            AddSummaryCell(summaryTable, "Pesanan Selesai", completedOrders.ToString(), boldFont, normalFont);
            AddSummaryCell(summaryTable, "Pesanan Pending", pendingOrders.ToString(), boldFont, normalFont);
            AddSummaryCell(summaryTable, "Total Pendapatan", $"Rp {totalRevenue:N0}", boldFont, normalFont);

            document.Add(summaryTable);
        }

        private void AddSummaryCell(PdfPTable table, string label, string value, Font boldFont, Font normalFont)
        {
            var cell = new PdfPCell();
            cell.Padding = 10;
            cell.BackgroundColor = new BaseColor(240, 248, 255);
            cell.AddElement(new Paragraph(label, normalFont));
            cell.AddElement(new Paragraph(value, boldFont));
            table.AddCell(cell);
        }

        private void AddTransactionTable(Document document, List<Order> orders, Font headerFont, Font normalFont, Font smallFont)
        {
            // Table header
            var tableTitle = new Paragraph("DETAIL TRANSAKSI", headerFont);
            tableTitle.SpacingBefore = 10;
            document.Add(tableTitle);

            if (orders == null || orders.Count == 0)
            {
                var noDataParagraph = new Paragraph("Tidak ada data transaksi untuk ditampilkan", normalFont);
                noDataParagraph.Alignment = Element.ALIGN_CENTER;
                noDataParagraph.SpacingBefore = 20;
                noDataParagraph.SpacingAfter = 20;
                document.Add(noDataParagraph);
                return;
            }

            // Create table
            var table = new PdfPTable(7);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 15, 20, 15, 15, 12, 12, 11 });

            // Table headers
            string[] headers = { "ID Pesanan", "Pelanggan", "Layanan", "Total", "Status", "Tanggal", "Berat (kg)" };
            foreach (string header in headers)
            {
                var headerCell = new PdfPCell(new Phrase(header, headerFont));
                headerCell.BackgroundColor = new BaseColor(70, 130, 180);
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell.Padding = 8;
                table.AddCell(headerCell);
            }

            // Table data - sort by creation date descending
            var sortedOrders = orders.OrderByDescending(o => o.CreatedAt).ToList();
            Console.WriteLine($"PdfService: Adding {sortedOrders.Count} rows to table");

            foreach (var order in sortedOrders)
            {
                AddTableCell(table, order.OrderId ?? "N/A", smallFont);
                AddTableCell(table, order.NamaLengkap ?? "N/A", smallFont);
                AddTableCell(table, order.JenisLayanan ?? "N/A", smallFont);
                AddTableCell(table, order.Total ?? "Rp 0", smallFont);
                AddTableCell(table, order.Status ?? "N/A", smallFont);
                AddTableCell(table, FormatDateForDisplay(order.TanggalPesan), smallFont);
                AddTableCell(table, order.Jumlah.ToString(), smallFont);
            }

            document.Add(table);
        }

        private void AddReportFooter(Document document, Font normalFont)
        {
            document.Add(new Paragraph(" "));
            
            var footer = new Paragraph($"Laporan digenerate pada: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", normalFont);
            footer.Alignment = Element.ALIGN_RIGHT;
            document.Add(footer);

            var signature = new Paragraph("Mengetahui,\n\n\n_________________\nManager", normalFont);
            signature.Alignment = Element.ALIGN_RIGHT;
            signature.SpacingBefore = 20;
            document.Add(signature);
        }
        public byte[] GenerateAllTransactionReportPdf(List<Order> orders)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Create document in landscape orientation for better table display
                var document = new Document(PageSize.A4.Rotate(), 30, 30, 30, 30);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                
                document.Open();

                // Register fonts
                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                var titleFont = new Font(baseFont, 16, Font.BOLD, new BaseColor(0, 0, 255));
                var headerFont = new Font(baseFont, 12, Font.BOLD, new BaseColor(0, 0, 0));
                var normalFont = new Font(baseFont, 9, Font.NORMAL, new BaseColor(0, 0, 0));
                var boldFont = new Font(baseFont, 9, Font.BOLD, new BaseColor(0, 0, 0));
                var smallFont = new Font(baseFont, 8, Font.NORMAL, new BaseColor(0, 0, 0));

                // Header with company info
                AddReportHeader(document, titleFont, normalFont);

                // Add spacing
                document.Add(new Paragraph(" "));

                // Report title
                var title = new Paragraph("LAPORAN TRANSAKSI LENGKAP", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // All data subtitle
                var subtitle = new Paragraph("Semua Data Transaksi", headerFont);
                subtitle.Alignment = Element.ALIGN_CENTER;
                document.Add(subtitle);

                document.Add(new Paragraph(" "));

                // Summary statistics
                AddReportSummary(document, orders, headerFont, boldFont, normalFont);

                document.Add(new Paragraph(" "));

                // Transaction table
                AddTransactionTable(document, orders, headerFont, normalFont, smallFont);

                // Footer
                AddReportFooter(document, normalFont);

                document.Close();
                return memoryStream.ToArray();
            }
        }
    }
}