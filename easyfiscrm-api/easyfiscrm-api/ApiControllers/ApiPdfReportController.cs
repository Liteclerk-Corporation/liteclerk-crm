using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace easyfiscrm_api.ApiControllers
{
    [Authorize, RoutePrefix("api/pdf/report")]
    public class ApiPdfReportController : ApiController
    {
        public Data.easyfiscrmdbDataContext db = new Data.easyfiscrmdbDataContext();
        private MemoryStream workStream = new MemoryStream();
        private Document document = new Document(new Rectangle(PageSize.LETTER), 72, 72, 72, 72);


        // ====================
        // PDF Customized Fonts
        // ====================
        private Font fontArial5Bold = FontFactory.GetFont("Arial", 5, Font.BOLD);
        private Font fontArial5 = FontFactory.GetFont("Arial", 5);
        private Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
        private Font fontArial9 = FontFactory.GetFont("Arial", 9);
        private Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
        private Font fontArial10BoldItalic = FontFactory.GetFont("Arial", 10, Font.BOLDITALIC, BaseColor.WHITE);
        private Font fontArial10 = FontFactory.GetFont("Arial", 10);
        private Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
        private Font fontArial11 = FontFactory.GetFont("Arial", 11);
        private Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
        private Font fontArial12BoldItalic = FontFactory.GetFont("Arial", 12, Font.BOLDITALIC);
        private Font fontArial12 = FontFactory.GetFont("Arial", 12);
        private Font fontArial12Italic = FontFactory.GetFont("Arial", 12, Font.ITALIC);
        private Font fontArial13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD);
        private Font fontArial13 = FontFactory.GetFont("Arial", 13);
        private Font fontArial14Bold = FontFactory.GetFont("Arial", 14, Font.BOLD);
        private Font fontArial14 = FontFactory.GetFont("Arial", 14);
        private Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
        private Font fontArial17 = FontFactory.GetFont("Arial", 17);
        private Font CourierNew11 = FontFactory.GetFont("Courier", 11);

        // ========
        // PDF Line
        // ========
        private Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

        [HttpGet, Route("list/lead/{id}")]
        public HttpResponseMessage PdfLead(string id)
        {
            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 40f, 30f, 40f);

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { PaddingTop = 5f, Border = 0 });

            // =============
            // Open Document
            // =============
            document.Open();

            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User not found!");
            }

            var lead = from d in db.CrmTrnLeads
                       where d.Id == Convert.ToInt32(id)
                       select d;

            if (lead.Any())
            {

                var company = from d in db.MstCompanies
                              where d.Id == currentUser.FirstOrDefault().CompanyId
                              select d;

                // =========
                // Lead Data
                // =========
                String CompanyName = company.FirstOrDefault().Company;
                String LeadNumber = lead.FirstOrDefault().LDNumber;
                String Date = lead.FirstOrDefault().LDDate.ToShortDateString();
                String Name = lead.FirstOrDefault().Name;
                String Address = lead.FirstOrDefault().Address;
                String ContactPerson = lead.FirstOrDefault().ContactPerson;
                String ContactPosition = lead.FirstOrDefault().ContactPosition;
                String ContactEmail = lead.FirstOrDefault().ContactEmail;
                String ContactPhoneNumber = lead.FirstOrDefault().ContactPhoneNumber;
                String ReferredBy = lead.FirstOrDefault().ReferredBy;
                String Remarks = lead.FirstOrDefault().Remarks;
                String AssignedToUser = lead.FirstOrDefault().MstUser.FullName;
                String Status = lead.FirstOrDefault().Status;
                String CreatedByUser = lead.FirstOrDefault().MstUser1.FullName;

                PdfPTable tableLead = new PdfPTable(2);
                tableLead.SetWidths(new float[] { 80f, 80f });
                tableLead.WidthPercentage = 100;

                PdfPCell colspan1 = new PdfPCell(new Phrase("Lead Transaction \n" + CompanyName));

                colspan1.Colspan = 2;
                colspan1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableLead.AddCell(colspan1);

                Phrase leadDetailNumberLabel = new Phrase("LD No: ", fontArial12);
                Phrase leadDetailNumber = new Phrase(LeadNumber + "\n", fontArial12Bold);

                Phrase leadDetailDateLabel = new Phrase("LD Date: ", fontArial12);
                Phrase leadDetailDate = new Phrase(Date + "\n", fontArial12Bold);

                Phrase leadDetailNameLabel = new Phrase("Lead's Name: ", fontArial12);
                Phrase leadDetailName = new Phrase(Name + "\n", fontArial12Bold);

                Phrase leadDetailAddressLabel = new Phrase("Address: ", fontArial12);
                Phrase leadDetailAddress = new Phrase(Address + "\n", fontArial12Bold);

                Phrase leadDetailContactPersonlabel = new Phrase("Contact Person: ", fontArial12);
                Phrase leadDetailContactPerson = new Phrase(ContactPerson + "\n", fontArial12Bold);

                Phrase leadDetailContactPositionLabel = new Phrase("Contact Position: ", fontArial12);
                Phrase leadDetailContactPosition = new Phrase(ContactPosition + "\n", fontArial12Bold);

                Phrase leadDetailContactContactEmailLabel = new Phrase("E-mail: ", fontArial12);
                Phrase leadDetailContactContactEmail = new Phrase(ContactEmail + "\n", fontArial12Bold);

                Phrase leadDetailContactContactPhoneNumberLabel = new Phrase("Contact Number: ", fontArial12);
                Phrase leadDetailContactContactPhoneNumber = new Phrase(ContactPhoneNumber + "\n", fontArial12Bold);

                Phrase leadDetailContactContactAssignedToUserLabel = new Phrase("Sales Assigned To: ", fontArial12);
                Phrase leadDetailContactContactAssignedToUser = new Phrase(AssignedToUser + "\n", fontArial12Bold);

                Phrase leadDetailContactContactCreatedByUserLabel = new Phrase("Created/Encoded By: ", fontArial12);
                Phrase leadDetailContactContactCreatedByUser = new Phrase(CreatedByUser + "\n", fontArial12Bold);

                Phrase leadDetailContactContactReferredByLabel = new Phrase("Referred By: ", fontArial12);
                Phrase leadDetailContactContactReferredBy = new Phrase(ReferredBy + "\n", fontArial12Bold);

                Phrase leadDetailContactContactStatusLabel = new Phrase("Status: ", fontArial12);
                Phrase leadDetailContactContactStatus = new Phrase(Status + "\n", fontArial12Bold);

                Phrase leadDetailContactContactRemarksLabel = new Phrase("Remarks: ", fontArial12);
                Phrase leadDetailContactContactRemarks = new Phrase(Remarks + "\n", fontArial12Bold);

                Paragraph leadDetailDetailColumn1 = new Paragraph
                {
                    leadDetailNumberLabel, leadDetailNumber,
                    leadDetailDateLabel, leadDetailDate,
                    leadDetailNameLabel, leadDetailName,
                    leadDetailAddressLabel, leadDetailAddress,
                    leadDetailContactPersonlabel, leadDetailContactPerson,
                    leadDetailContactPositionLabel, leadDetailContactPosition,
                    leadDetailContactContactEmailLabel, leadDetailContactContactEmail,
                    leadDetailContactContactPhoneNumberLabel, leadDetailContactContactPhoneNumber
                };

                Paragraph leadDetailDetailColumn2 = new Paragraph
                {
                    leadDetailContactContactAssignedToUserLabel, leadDetailContactContactAssignedToUser,
                    leadDetailContactContactCreatedByUserLabel, leadDetailContactContactCreatedByUser,
                    leadDetailContactContactReferredByLabel, leadDetailContactContactReferredBy,
                    leadDetailContactContactStatusLabel, leadDetailContactContactStatus,
                    leadDetailContactContactRemarksLabel, leadDetailContactContactRemarks,
                };

                tableLead.AddCell(new PdfPCell(leadDetailDetailColumn1) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                tableLead.AddCell(new PdfPCell(leadDetailDetailColumn2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                var activities = from d in db.CrmTrnActivities
                                 where d.LDId == Convert.ToInt32(id)
                                 select new Entities.CrmTrnActivityEntity
                                 {
                                     Id = d.Id,
                                     ACNumber = d.ACNumber,
                                     ACDate = d.ACDate.ToShortDateString(),
                                     UserId = d.UserId,
                                     User = d.MstUser.FullName,
                                     FunctionalUserId = d.FunctionalUserId,
                                     FunctionalUser = d.MstUser1.FullName,
                                     TechnicalUserId = d.TechnicalUserId,
                                     TechnicalUser = d.MstUser2.FullName,
                                     CRMStatus = d.CRMStatus,
                                     Activity = d.Activity,
                                     StartDate = d.StartDateTime.ToShortDateString(),
                                     StartTime = d.StartDateTime.ToShortTimeString(),
                                     EndDate = d.EndDateTime.ToShortDateString(),
                                     EndTime = d.EndDateTime.ToShortTimeString(),
                                     TransportationCost = d.TransportationCost,
                                     OnSiteCost = d.OnSiteCost,
                                     LDId = d.LDId,
                                     SDId = d.SDId,
                                     SPId = d.SPId,
                                     Status = d.Status,
                                     IsLocked = d.IsLocked,
                                     CreatedByUserId = d.CreatedByUserId,
                                     CreatedByUser = d.MstUser3.FullName,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedByUserId = d.UpdatedByUserId,
                                     UpdatedByUser = d.MstUser4.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };

                PdfPTable tableLeadActivity = new PdfPTable(7);
                tableLeadActivity.SetWidths(new float[] { 80f, 80f, 80f, 80f, 80f, 80f, 80f });
                tableLeadActivity.WidthPercentage = 100;

                PdfPCell tableLeadActivitColSpan = new PdfPCell(new Phrase("Activity"));

                tableLeadActivitColSpan.Colspan = 7;
                tableLeadActivitColSpan.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                tableLeadActivity.AddCell(new PdfPCell(tableLeadActivitColSpan) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 3f, Border = 0 });

                Phrase leadDetailActivityHeaderDate = new Phrase("Date", fontArial9);
                Phrase leadDetailActivityHeaderFunctionalAssigned = new Phrase("Functional Assigned", fontArial9);
                Phrase leadDetailActivityHeaderTechnicalStaffAssigned = new Phrase("Technical Staff Assigned", fontArial9);
                Phrase leadDetailActivityHeaderStatus = new Phrase("Status", fontArial9);
                Phrase leadDetailActivityHeaderActivity = new Phrase("Activity", fontArial9);
                Phrase leadDetailActivityHeaderTransportationFee = new Phrase("Transportation Fee", fontArial9);
                Phrase leadDetailActivityHeaderOnsiteFee = new Phrase("On-site Fee", fontArial9);

                tableLeadActivity.AddCell(leadDetailActivityHeaderDate);
                tableLeadActivity.AddCell(leadDetailActivityHeaderFunctionalAssigned);
                tableLeadActivity.AddCell(leadDetailActivityHeaderTechnicalStaffAssigned);
                tableLeadActivity.AddCell(leadDetailActivityHeaderStatus);
                tableLeadActivity.AddCell(leadDetailActivityHeaderActivity);
                tableLeadActivity.AddCell(leadDetailActivityHeaderTransportationFee);
                tableLeadActivity.AddCell(leadDetailActivityHeaderOnsiteFee);

                foreach (var activity in activities)
                {
                    Phrase leadDetailActivityDate = new Phrase(activity.ACDate, fontArial9);
                    Phrase leadDetailActivityFunctionalAssigned = new Phrase(activity.FunctionalUser, fontArial9);
                    Phrase leadDetailActivityTechnicalStaffAssigned = new Phrase(activity.TechnicalUser, fontArial9);
                    Phrase leadDetailActivityStatus = new Phrase(activity.Status, fontArial9);
                    Phrase leadDetailActivityActivity = new Phrase(activity.Activity, fontArial9);
                    Phrase leadDetailActivityTransportationFee = new Phrase(activity.TransportationCost.ToString("#,##0.00"), fontArial9);
                    Phrase leadDetailActivityOnsiteFee = new Phrase(activity.OnSiteCost.ToString("#,##0.00"), fontArial9);

                    tableLeadActivity.AddCell(leadDetailActivityDate);
                    tableLeadActivity.AddCell(leadDetailActivityFunctionalAssigned);
                    tableLeadActivity.AddCell(leadDetailActivityTechnicalStaffAssigned);
                    tableLeadActivity.AddCell(leadDetailActivityStatus);
                    tableLeadActivity.AddCell(leadDetailActivityActivity);
                    tableLeadActivity.AddCell(new PdfPCell(leadDetailActivityTransportationFee) { HorizontalAlignment = 2 });
                    tableLeadActivity.AddCell(new PdfPCell(leadDetailActivityOnsiteFee) { HorizontalAlignment = 2 });
                }

                PdfPCell colspan2 = new PdfPCell(tableLeadActivity);
                colspan2.Colspan = 2;
                colspan2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableLead.AddCell(new PdfPCell(colspan2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase leadDetailCheckedby = new Phrase("\n \n Checked By: ___________________________", fontArial12);
                Phrase leadDetailApprovedby = new Phrase("\n \n Approved By: ___________________________", fontArial12);

                Paragraph leadDetailCheckedByColumn1 = new Paragraph
                {
                    leadDetailCheckedby
                };

                Paragraph leadDetailApprovedByColumn2 = new Paragraph
                {
                    leadDetailApprovedby
                };

                tableLead.AddCell(new PdfPCell(leadDetailCheckedByColumn1) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                tableLead.AddCell(new PdfPCell(leadDetailApprovedByColumn2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(tableLead);
            }

            // ==============
            // Close Document
            // ==============
            document.Close();

            // ===============
            // Response Stream
            // ===============
            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=customer.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        [HttpGet, Route("list/salesDelivery/{id}")]
        public HttpResponseMessage PdfSalesDelivery(string id)
        {
            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 40f, 30f, 40f);

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { PaddingTop = 5f, Border = 0 });

            // =============
            // Open Document
            // =============
            document.Open();

            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User not found!");
            }

            var salesDelivery = from d in db.CrmTrnSalesDeliveries
                                where d.Id == Convert.ToInt32(id)
                                select d;

            if (salesDelivery.Any())
            {

                var company = from d in db.MstCompanies
                              where d.Id == currentUser.FirstOrDefault().CompanyId
                              select d;

                // =========
                // Lead Data
                // =========
                String CompanyName = company.FirstOrDefault().Company;
                String SDNumber = salesDelivery.FirstOrDefault().SDNumber;
                String Date = salesDelivery.FirstOrDefault().SDDate.ToShortDateString();
                String Customer = salesDelivery.FirstOrDefault().MstArticle.Article;
                String Address = salesDelivery.FirstOrDefault().MstArticle.Address;
                String ContactPerson = salesDelivery.FirstOrDefault().ContactPerson;
                String ContactPosition = salesDelivery.FirstOrDefault().ContactPosition;
                String ContactEmail = salesDelivery.FirstOrDefault().ContactEmail;
                String ContactPhoneNumber = salesDelivery.FirstOrDefault().ContactPhoneNumber;
                String LDNumber = salesDelivery.FirstOrDefault().CrmTrnLead.LDNumber;
                String AssignedToUser = salesDelivery.FirstOrDefault().MstUser.FullName;
                String CreatedByUser = salesDelivery.FirstOrDefault().MstUser1.FullName;
                String Product = salesDelivery.FirstOrDefault().CrmMstProduct.ProductDescription;
                String Status = salesDelivery.FirstOrDefault().Status;
                String Remarks = salesDelivery.FirstOrDefault().Particulars;


                PdfPTable tableSalesDelivery = new PdfPTable(2);
                tableSalesDelivery.SetWidths(new float[] { 80f, 80f });
                tableSalesDelivery.WidthPercentage = 100;

                PdfPCell colspan1 = new PdfPCell(new Phrase("Sales Delivery Transaction \n" + CompanyName));

                colspan1.Colspan = 2;
                colspan1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableSalesDelivery.AddCell(colspan1);

                Phrase salesDeliveryDetailNumberlabel = new Phrase("SD No.: ", fontArial12);
                Phrase salesDeliveryDetailNumber = new Phrase( SDNumber + "\n", fontArial12Bold);

                
                Phrase salesDeliveryDetailDateLabel = new Phrase("SD Date: ", fontArial12);
                Phrase salesDeliveryDetailDate = new Phrase(Date + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailNameLabel = new Phrase("Customer: ", fontArial12);
                Phrase salesDeliveryDetailName = new Phrase(Customer + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailAddresslabel = new Phrase("Address: ", fontArial12);
                Phrase salesDeliveryDetailAddress = new Phrase(Address + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailContactPersonLabel = new Phrase("Contact Person: ", fontArial12);
                Phrase salesDeliveryDetailContactPerson = new Phrase(ContactPerson + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailContactPositionLabel = new Phrase("Contact Position: ", fontArial12);
                Phrase salesDeliveryDetailContactPosition = new Phrase(ContactPosition + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailContactContactEmailLabel = new Phrase("E-mail: ", fontArial12);
                Phrase salesDeliveryDetailContactContactEmail = new Phrase(ContactEmail + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailContactContactPhoneNumberLabel = new Phrase("Contact Number: ", fontArial12);
                Phrase salesDeliveryDetailContactContactPhoneNumber = new Phrase(ContactPhoneNumber + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailContactContactLDNumberLabel = new Phrase("LD Number: ", fontArial12);
                Phrase salesDeliveryDetailContactContactLDNumber = new Phrase("LD Number: " + LDNumber + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailContactContactAssignedToUserLabel = new Phrase("Sales Assigned To: ", fontArial12);
                Phrase salesDeliveryDetailContactContactAssignedToUser = new Phrase(AssignedToUser + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailContactContactCreatedByUserLabel = new Phrase("Created/Encoded By: ", fontArial12);
                Phrase salesDeliveryDetailContactContactCreatedByUser = new Phrase( CreatedByUser + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailContactContactReProductLabel = new Phrase("Product: ", fontArial12);
                Phrase salesDeliveryDetailContactContactReProduct = new Phrase(Product + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailContactContactStatusLabel = new Phrase("Status: ", fontArial12);
                Phrase salesDeliveryDetailContactContactStatus = new Phrase(Status + "\n", fontArial12Bold);

                Phrase salesDeliveryDetailContactContactRemarkslabel = new Phrase("Remarks: ", fontArial12);
                Phrase salesDeliveryDetailContactContactRemarks = new Phrase( Remarks + "\n", fontArial12Bold);

                Paragraph salesDeliveryDetailDetailColumn1 = new Paragraph
                {
                    salesDeliveryDetailNumberlabel, salesDeliveryDetailNumber,
                    salesDeliveryDetailDateLabel, salesDeliveryDetailDate,
                    salesDeliveryDetailNameLabel, salesDeliveryDetailName,
                    salesDeliveryDetailAddresslabel, salesDeliveryDetailAddress,
                    salesDeliveryDetailContactPersonLabel, salesDeliveryDetailContactPerson,
                    salesDeliveryDetailContactPositionLabel, salesDeliveryDetailContactPosition,
                    salesDeliveryDetailContactContactEmailLabel, salesDeliveryDetailContactContactEmail,
                    salesDeliveryDetailContactContactPhoneNumberLabel, salesDeliveryDetailContactContactPhoneNumber,
                    salesDeliveryDetailContactContactLDNumberLabel, salesDeliveryDetailContactContactLDNumber
                };

                Paragraph salesDeliveryDetailDetailColumn2 = new Paragraph
                {
                    salesDeliveryDetailContactContactAssignedToUserLabel, salesDeliveryDetailContactContactAssignedToUser,
                    salesDeliveryDetailContactContactCreatedByUserLabel, salesDeliveryDetailContactContactCreatedByUser,
                    salesDeliveryDetailContactContactReProductLabel, salesDeliveryDetailContactContactReProduct,
                    salesDeliveryDetailContactContactStatusLabel, salesDeliveryDetailContactContactStatus,
                    salesDeliveryDetailContactContactRemarkslabel, salesDeliveryDetailContactContactRemarks,
                };

                tableSalesDelivery.AddCell(new PdfPCell(salesDeliveryDetailDetailColumn1) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                tableSalesDelivery.AddCell(new PdfPCell(salesDeliveryDetailDetailColumn2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                var activities = from d in db.CrmTrnActivities
                                 where d.SDId == Convert.ToInt32(id)
                                 select new Entities.CrmTrnActivityEntity
                                 {
                                     Id = d.Id,
                                     ACNumber = d.ACNumber,
                                     ACDate = d.ACDate.ToShortDateString(),
                                     UserId = d.UserId,
                                     User = d.MstUser.FullName,
                                     FunctionalUserId = d.FunctionalUserId,
                                     FunctionalUser = d.MstUser1.FullName,
                                     TechnicalUserId = d.TechnicalUserId,
                                     TechnicalUser = d.MstUser2.FullName,
                                     CRMStatus = d.CRMStatus,
                                     Activity = d.Activity,
                                     StartDate = d.StartDateTime.ToShortDateString(),
                                     StartTime = d.StartDateTime.ToShortTimeString(),
                                     EndDate = d.EndDateTime.ToShortDateString(),
                                     EndTime = d.EndDateTime.ToShortTimeString(),
                                     TransportationCost = d.TransportationCost,
                                     OnSiteCost = d.OnSiteCost,
                                     LDId = d.LDId,
                                     SDId = d.SDId,
                                     SPId = d.SPId,
                                     Status = d.Status,
                                     IsLocked = d.IsLocked,
                                     CreatedByUserId = d.CreatedByUserId,
                                     CreatedByUser = d.MstUser3.FullName,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedByUserId = d.UpdatedByUserId,
                                     UpdatedByUser = d.MstUser4.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };

                PdfPTable tableSalesDeliveryActivity = new PdfPTable(7);
                tableSalesDeliveryActivity.SetWidths(new float[] { 50f, 80f, 80f, 50f, 140f, 65f, 65f });
                tableSalesDeliveryActivity.WidthPercentage = 100;

                PdfPCell tableSalesDeliveryActivitColSpan = new PdfPCell(new Phrase("Activity"));

                tableSalesDeliveryActivitColSpan.Colspan = 7;
                tableSalesDeliveryActivitColSpan.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                tableSalesDeliveryActivity.AddCell(new PdfPCell(tableSalesDeliveryActivitColSpan) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 3f, Border = 0 });

                Phrase salesDeliveryDetailActivityHeaderDate = new Phrase("Date", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderFunctionalAssigned = new Phrase("Functional Assigned", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderTechnicalStaffAssigned = new Phrase("Technical Staff Assigned", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderStatus = new Phrase("Status", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderActivity = new Phrase("Activity", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderTransportationFee = new Phrase("Transportation Fee", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderOnsiteFee = new Phrase("On-site Fee", fontArial9);

                tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityHeaderDate);
                tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityHeaderFunctionalAssigned);
                tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityHeaderTechnicalStaffAssigned);
                tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityHeaderStatus);
                tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityHeaderActivity);
                tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityHeaderTransportationFee);
                tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityHeaderOnsiteFee);

                foreach (var activity in activities)
                {
                    Phrase salesDeliveryDetailActivityDate = new Phrase(activity.ACDate, fontArial9);
                    Phrase salesDeliveryDetailActivityFunctionalAssigned = new Phrase(activity.FunctionalUser, fontArial9);
                    Phrase salesDeliveryDetailActivityTechnicalStaffAssigned = new Phrase(activity.TechnicalUser, fontArial9);
                    Phrase salesDeliveryDetailActivityStatus = new Phrase(activity.Status, fontArial9);
                    Phrase salesDeliveryDetailActivityActivity = new Phrase(activity.Activity, fontArial9);
                    Phrase salesDeliveryDetailActivityTransportationFee = new Phrase(activity.TransportationCost.ToString("#,##0.00"), fontArial9);
                    Phrase salesDeliveryDetailActivityOnsiteFee = new Phrase(activity.OnSiteCost.ToString("#,##0.00"), fontArial9);

                    tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityDate);
                    tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityFunctionalAssigned);
                    tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityTechnicalStaffAssigned);
                    tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityStatus);
                    tableSalesDeliveryActivity.AddCell(salesDeliveryDetailActivityActivity);
                    tableSalesDeliveryActivity.AddCell(new PdfPCell(salesDeliveryDetailActivityTransportationFee) { HorizontalAlignment = 2 });
                    tableSalesDeliveryActivity.AddCell(new PdfPCell(salesDeliveryDetailActivityOnsiteFee) { HorizontalAlignment = 2 });
                }

                PdfPCell colspan2 = new PdfPCell(tableSalesDeliveryActivity);
                colspan2.Colspan = 2;
                colspan2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableSalesDelivery.AddCell(new PdfPCell(colspan2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase salesDeliveryDetailCheckedby = new Phrase("\n \n Checked By: ___________________________", fontArial12);
                Phrase salesDeliveryDetailApprovedby = new Phrase("\n \n Approved By: ___________________________", fontArial12);

                Paragraph salesDeliveryDetailCheckedByColumn1 = new Paragraph
                {
                    salesDeliveryDetailCheckedby
                };

                Paragraph salesDeliveryDetailApprovedByColumn2 = new Paragraph
                {
                    salesDeliveryDetailApprovedby
                };

                tableSalesDelivery.AddCell(new PdfPCell(salesDeliveryDetailCheckedByColumn1) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                tableSalesDelivery.AddCell(new PdfPCell(salesDeliveryDetailApprovedByColumn2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(tableSalesDelivery);
            }

            // ==============
            // Close Document
            // ==============
            document.Close();

            // ===============
            // Response Stream
            // ===============
            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=customer.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        [HttpGet, Route("list/support/{id}")]
        public HttpResponseMessage PdfSupportDelivery(string id)
        {
            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 40f, 30f, 40f);

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { PaddingTop = 5f, Border = 0 });

            // =============
            // Open Document
            // =============
            document.Open();

            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User not found!");
            }

            var support = from d in db.CrmTrnSupports
                          where d.Id == Convert.ToInt32(id)
                          select d;

            if (support.Any())
            {

                var company = from d in db.MstCompanies
                              where d.Id == currentUser.FirstOrDefault().CompanyId
                              select d;

                // =========
                // Lead Data
                // =========
                String CompanyName = company.FirstOrDefault().Company;
                String SPNumber = support.FirstOrDefault().SPNumber;
                String Date = support.FirstOrDefault().SPDate.ToShortDateString();
                String Customer = support.FirstOrDefault().MstArticle.Article;
                String Address = support.FirstOrDefault().MstArticle.Address;
                String ContactPerson = support.FirstOrDefault().ContactPerson;
                String ContactPosition = support.FirstOrDefault().ContactPosition;
                String ContactEmail = support.FirstOrDefault().ContactEmail;
                String ContactPhoneNumber = support.FirstOrDefault().ContactPhoneNumber;
                String SDNumber = support.FirstOrDefault().CrmTrnSalesDelivery.SDNumber;
                String AssignedToUser = support.FirstOrDefault().MstUser.FullName;
                String CreatedByUser = support.FirstOrDefault().MstUser1.FullName;
                String Product = support.FirstOrDefault().CrmTrnSalesDelivery.CrmMstProduct.ProductDescription;
                String Status = support.FirstOrDefault().Status;
                String Issue = support.FirstOrDefault().Issue;


                PdfPTable tableSupport = new PdfPTable(2);
                tableSupport.SetWidths(new float[] { 80f, 80f });
                tableSupport.WidthPercentage = 100;

                PdfPCell colspan1 = new PdfPCell(new Phrase("Support Transaction \n" + CompanyName));

                colspan1.Colspan = 2;
                colspan1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableSupport.AddCell(colspan1);

                Phrase supportDetailNumberLabel = new Phrase("SP No.: ", fontArial12);
                Phrase supportDetailNumber = new Phrase(SDNumber + "\n", fontArial12Bold);

                Phrase supportDetailDateLabel = new Phrase("SP Date: ", fontArial12);
                Phrase supportDetailDate = new Phrase(Date + "\n", fontArial12Bold);

                Phrase supportDetailNameLabel = new Phrase("Customer: ", fontArial12);
                Phrase supportDetailName = new Phrase(Customer + "\n", fontArial12Bold);

                Phrase supportDetailAddressLabel = new Phrase("Address: ", fontArial12);
                Phrase supportDetailAddress = new Phrase(Address + "\n", fontArial12Bold);

                Phrase supportDetailContactPersonLabel = new Phrase("Contact Person: ", fontArial12);
                Phrase supportDetailContactPerson = new Phrase(ContactPerson + "\n", fontArial12Bold);

                Phrase supportDetailContactPositionLabel = new Phrase("Contact Position: ", fontArial12);
                Phrase supportDetailContactPosition = new Phrase(ContactPosition + "\n", fontArial12Bold);

                Phrase supportDetailContactContactEmailLabel = new Phrase("E-mail: ", fontArial12);
                Phrase supportDetailContactContactEmail = new Phrase(ContactEmail + "\n", fontArial12Bold);

                Phrase supportDetailContactContactPhoneNumberLabel = new Phrase("Contact Number: ", fontArial12);
                Phrase supportDetailContactContactPhoneNumber = new Phrase(ContactPhoneNumber + "\n", fontArial12Bold);

                Phrase supportDetailContactContactLDNumberLabel = new Phrase("SD Number: ", fontArial12);
                Phrase supportDetailContactContactLDNumber = new Phrase(SDNumber + "\n", fontArial12Bold);

                Phrase supportDetailContactContactAssignedToUserLabel = new Phrase("Sales Assigned To: ", fontArial12);
                Phrase supportDetailContactContactAssignedToUser = new Phrase(AssignedToUser + "\n", fontArial12Bold);

                Phrase supportDetailContactContactCreatedByUserLabel = new Phrase("Created/Encoded By: ", fontArial12);
                Phrase supportDetailContactContactCreatedByUser = new Phrase(CreatedByUser + "\n", fontArial12Bold);

                Phrase supportDetailReProductLabel = new Phrase("Product: ", fontArial12);
                Phrase supportDetailReProduct = new Phrase(Product + "\n", fontArial12Bold);

                Phrase supportDetailContactContactStatusLabel = new Phrase("Status: ", fontArial12);
                Phrase supportDetailContactContactStatus = new Phrase(Status + "\n", fontArial12Bold);

                Phrase supportDetailContactContactRemarksLabel = new Phrase("Issue: ", fontArial12);
                Phrase supportDetailContactContactRemarks = new Phrase(Issue + "\n", fontArial12Bold);

                Paragraph supportDetailDetailColumn1 = new Paragraph
                {
                    supportDetailNumberLabel, supportDetailNumber,
                    supportDetailDateLabel, supportDetailDate,
                    supportDetailNameLabel, supportDetailName,
                    supportDetailAddressLabel, supportDetailAddress,
                    supportDetailContactPersonLabel, supportDetailContactPerson,
                    supportDetailContactPositionLabel, supportDetailContactPosition,
                    supportDetailContactContactEmailLabel, supportDetailContactContactEmail,
                    supportDetailContactContactPhoneNumberLabel, supportDetailContactContactPhoneNumber,
                    supportDetailContactContactLDNumberLabel, supportDetailContactContactLDNumber
                };

                Paragraph supportDetailDetailColumn2 = new Paragraph
                {
                    supportDetailContactContactAssignedToUserLabel, supportDetailContactContactAssignedToUser,
                    supportDetailContactContactCreatedByUserLabel, supportDetailContactContactCreatedByUser,
                    supportDetailReProductLabel, supportDetailReProduct,
                    supportDetailContactContactStatusLabel, supportDetailContactContactStatus,
                    supportDetailContactContactRemarksLabel, supportDetailContactContactRemarks
                };

                tableSupport.AddCell(new PdfPCell(supportDetailDetailColumn1) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                tableSupport.AddCell(new PdfPCell(supportDetailDetailColumn2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                var activities = from d in db.CrmTrnActivities
                                 where d.SPId == Convert.ToInt32(id)
                                 select new Entities.CrmTrnActivityEntity
                                 {
                                     Id = d.Id,
                                     ACNumber = d.ACNumber,
                                     ACDate = d.ACDate.ToShortDateString(),
                                     UserId = d.UserId,
                                     User = d.MstUser.FullName,
                                     FunctionalUserId = d.FunctionalUserId,
                                     FunctionalUser = d.MstUser1.FullName,
                                     TechnicalUserId = d.TechnicalUserId,
                                     TechnicalUser = d.MstUser2.FullName,
                                     CRMStatus = d.CRMStatus,
                                     Activity = d.Activity,
                                     StartDate = d.StartDateTime.ToShortDateString(),
                                     StartTime = d.StartDateTime.ToShortTimeString(),
                                     EndDate = d.EndDateTime.ToShortDateString(),
                                     EndTime = d.EndDateTime.ToShortTimeString(),
                                     TransportationCost = d.TransportationCost,
                                     OnSiteCost = d.OnSiteCost,
                                     LDId = d.LDId,
                                     SDId = d.SDId,
                                     SPId = d.SPId,
                                     Status = d.Status,
                                     IsLocked = d.IsLocked,
                                     CreatedByUserId = d.CreatedByUserId,
                                     CreatedByUser = d.MstUser3.FullName,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedByUserId = d.UpdatedByUserId,
                                     UpdatedByUser = d.MstUser4.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };

                PdfPTable tableSupportActivity = new PdfPTable(7);
                tableSupportActivity.SetWidths(new float[] { 50f, 80f, 80f, 50f, 140f, 65f, 65f });
                tableSupportActivity.WidthPercentage = 100;

                PdfPCell tableSupportActivitColSpan = new PdfPCell(new Phrase("Activity"));

                tableSupportActivitColSpan.Colspan = 7;
                tableSupportActivitColSpan.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                tableSupportActivity.AddCell(new PdfPCell(tableSupportActivitColSpan) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 3f, Border = 0 });

                Phrase salesDeliveryDetailActivityHeaderDate = new Phrase("Date", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderFunctionalAssigned = new Phrase("Functional Assigned", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderTechnicalStaffAssigned = new Phrase("Technical Staff Assigned", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderStatus = new Phrase("Status", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderActivity = new Phrase("Activity", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderTransportationFee = new Phrase("Transportation Fee", fontArial9);
                Phrase salesDeliveryDetailActivityHeaderOnsiteFee = new Phrase("On-site Fee", fontArial9);

                tableSupportActivity.AddCell(salesDeliveryDetailActivityHeaderDate);
                tableSupportActivity.AddCell(salesDeliveryDetailActivityHeaderFunctionalAssigned);
                tableSupportActivity.AddCell(salesDeliveryDetailActivityHeaderTechnicalStaffAssigned);
                tableSupportActivity.AddCell(salesDeliveryDetailActivityHeaderStatus);
                tableSupportActivity.AddCell(salesDeliveryDetailActivityHeaderActivity);
                tableSupportActivity.AddCell(salesDeliveryDetailActivityHeaderTransportationFee);
                tableSupportActivity.AddCell(salesDeliveryDetailActivityHeaderOnsiteFee);

                foreach (var activity in activities)
                {
                    Phrase salesDeliveryDetailActivityDate = new Phrase(activity.ACDate, fontArial9);
                    Phrase salesDeliveryDetailActivityFunctionalAssigned = new Phrase(activity.FunctionalUser, fontArial9);
                    Phrase salesDeliveryDetailActivityTechnicalStaffAssigned = new Phrase(activity.TechnicalUser, fontArial9);
                    Phrase salesDeliveryDetailActivityStatus = new Phrase(activity.Status, fontArial9);
                    Phrase salesDeliveryDetailActivityActivity = new Phrase(activity.Activity, fontArial9);
                    Phrase salesDeliveryDetailActivityTransportationFee = new Phrase(activity.TransportationCost.ToString("#,##0.00"), fontArial9);
                    Phrase salesDeliveryDetailActivityOnsiteFee = new Phrase(activity.OnSiteCost.ToString("#,##0.00"), fontArial9);

                    tableSupportActivity.AddCell(salesDeliveryDetailActivityDate);
                    tableSupportActivity.AddCell(salesDeliveryDetailActivityFunctionalAssigned);
                    tableSupportActivity.AddCell(salesDeliveryDetailActivityTechnicalStaffAssigned);
                    tableSupportActivity.AddCell(salesDeliveryDetailActivityStatus);
                    tableSupportActivity.AddCell(salesDeliveryDetailActivityActivity);
                    tableSupportActivity.AddCell(new PdfPCell(salesDeliveryDetailActivityTransportationFee) { HorizontalAlignment = 2 });
                    tableSupportActivity.AddCell(new PdfPCell(salesDeliveryDetailActivityOnsiteFee) { HorizontalAlignment = 2 });
                }

                PdfPCell colspan2 = new PdfPCell(tableSupportActivity);
                colspan2.Colspan = 2;
                colspan2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableSupport.AddCell(new PdfPCell(colspan2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase supportDetailCheckedby = new Phrase("\n \n Checked By: ___________________________", fontArial12);
                Phrase supportDetailApprovedby = new Phrase("\n \n Approved By: ___________________________", fontArial12);

                Paragraph supportDetailCheckedByColumn1 = new Paragraph
                {
                    supportDetailCheckedby
                };

                Paragraph supportDetailApprovedByColumn2 = new Paragraph
                {
                    supportDetailApprovedby
                };

                tableSupport.AddCell(new PdfPCell(supportDetailCheckedByColumn1) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                tableSupport.AddCell(new PdfPCell(supportDetailApprovedByColumn2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(tableSupport);
            }

            // ==============
            // Close Document
            // ==============
            document.Close();

            // ===============
            // Response Stream
            // ===============
            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=customer.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }


        [HttpGet, Route("lead/activity/{id}")]
        public HttpResponseMessage PdfLeadActivity(string id)
        {
            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 40f, 30f, 40f);

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { PaddingTop = 5f, Border = 0 });

            // =============
            // Open Document
            // =============
            document.Open();

            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User not found!");
            }

            var leadActivity = from d in db.CrmTrnActivities
                       where d.Id == Convert.ToInt32(id)
                       select d;

            if (leadActivity.Any())
            {

                var company = from d in db.MstCompanies
                              where d.Id == currentUser.FirstOrDefault().CompanyId
                              select d;

                // =========
                // Lead Data
                // =========
                String CompanyName = company.FirstOrDefault().Company;
                String LeadNumber = leadActivity.FirstOrDefault().CrmTrnLead.LDNumber;
                String Date = leadActivity.FirstOrDefault().CrmTrnLead.LDDate.ToShortDateString();
                String Name = leadActivity.FirstOrDefault().CrmTrnLead.Name;
                String Product = "";
                String FunctionalStaff = leadActivity.FirstOrDefault().MstUser1.FullName;
                String TechnicalStaff = leadActivity.FirstOrDefault().MstUser2.FullName;
                String DateStart = leadActivity.FirstOrDefault().StartDateTime.ToShortDateString();
                String DateEnd = leadActivity.FirstOrDefault().EndDateTime.ToShortDateString();
                String Status = leadActivity.FirstOrDefault().CrmTrnLead.Status;
                String EncodedBy = leadActivity.FirstOrDefault().MstUser3.FullName;
                String ACNumber = leadActivity.FirstOrDefault().ACNumber;
                String Activity = leadActivity.FirstOrDefault().Activity;
                String TransportationFee = leadActivity.FirstOrDefault().TransportationCost.ToString("#,##0.00");
                String OnSiteCost = leadActivity.FirstOrDefault().OnSiteCost.ToString("#,##0.00");
                Decimal totalCost = leadActivity.FirstOrDefault().TransportationCost + leadActivity.FirstOrDefault().OnSiteCost;
                String totalCostString = totalCost.ToString("#,##0.00");

                PdfPTable tableLead = new PdfPTable(2);
                tableLead.SetWidths(new float[] { 80f, 80f });
                tableLead.WidthPercentage = 100;

                PdfPCell colspan1 = new PdfPCell(new Phrase("Lead Transaction \n" + CompanyName));

                colspan1.Colspan = 2;
                colspan1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableLead.AddCell(colspan1);

                Phrase ActivityDetailLDNumberLabel = new Phrase("LD No: ", fontArial12);
                Phrase ActivityDetailLDNumber = new Phrase(LeadNumber + "\n", fontArial12Bold);

                Phrase ActivityDetailDateLabel = new Phrase("LD Date: ", fontArial12);
                Phrase ActivityDetailDate = new Phrase(Date + "\n", fontArial12Bold);

                Phrase ActivityDetailNameLabel = new Phrase("Lead's Name: ", fontArial12);
                Phrase ActivityDetailName = new Phrase(Name + "\n", fontArial12Bold);

                Phrase ActivityDetailProductlabel = new Phrase("Product: ", fontArial12);
                Phrase ActivityDetailProduct = new Phrase(Product + "\n", fontArial12Bold);

                Phrase ActivityDetailFunctionalStaffLabel = new Phrase("Functional Staff: ", fontArial12);
                Phrase ActivityDetailFunctionalStaff = new Phrase(FunctionalStaff + "\n", fontArial12Bold);

                Phrase ActivityDetailTechnicalStaffLabel = new Phrase("Technical Staff: ", fontArial12);
                Phrase ActivityDetailTechnicalStaff = new Phrase(TechnicalStaff + "\n", fontArial12Bold);

                Phrase ActivityDetailDateStartrLabel = new Phrase("Date Start: ", fontArial12);
                Phrase ActivityDetailDateStart = new Phrase(DateStart + "\n", fontArial12Bold);

                Phrase ActivityDetailDateEndLabel = new Phrase("Date End: ", fontArial12);
                Phrase ActivityDetailDateEnd = new Phrase(DateEnd + "\n", fontArial12Bold);

                Phrase ActivityDetailStatusLabel = new Phrase("Status: ", fontArial12);
                Phrase ActivityDetailStatus = new Phrase(Status + "\n", fontArial12Bold);

                Phrase ActivityDetailEncodedByLabel = new Phrase("EncodedBy: ", fontArial12);
                Phrase ActivityDetailEncodedBy = new Phrase(EncodedBy + "\n", fontArial12Bold);

                Phrase ActivityDetailACNumberLabel = new Phrase("ACNumber: ", fontArial12);
                Phrase ActivityDetailACNumber = new Phrase(ACNumber + "\n", fontArial12Bold);

                Phrase ActivityDetailActivityLabel = new Phrase("Activity: ", fontArial12);
                Phrase ActivityDetailActivity = new Phrase(Activity + "\n", fontArial12Bold);

                Phrase ActivityDetailTransportationFeeLabel = new Phrase("Transportation Fee: ", fontArial12);
                Phrase ActivityDetailTransportationFee = new Phrase(TransportationFee + "\n", fontArial12Bold);

                Phrase ActivityDetailOnSiteCostLabel = new Phrase("On Site Cost: ", fontArial12);
                Phrase ActivityDetailOnSiteCost = new Phrase(OnSiteCost + "\n", fontArial12Bold);

                Phrase ActivityDetailTotalCostLabel = new Phrase("Total Cos: ", fontArial12);
                Phrase ActivityDetailTotalCost = new Phrase(totalCost.ToString("#,##0.00") + "\n", fontArial12Bold);

                Paragraph ActivityDetailDetailColumn1 = new Paragraph
                {
                    ActivityDetailLDNumberLabel, ActivityDetailLDNumber,
                    ActivityDetailDateLabel, ActivityDetailDate,
                    ActivityDetailNameLabel, ActivityDetailName,
                    ActivityDetailProductlabel, ActivityDetailProduct,
                    ActivityDetailFunctionalStaffLabel, ActivityDetailFunctionalStaff,
                    ActivityDetailTechnicalStaffLabel, ActivityDetailTechnicalStaff,
                    ActivityDetailDateStartrLabel, ActivityDetailDateStart,
                    ActivityDetailDateEndLabel, ActivityDetailDateEnd,
                    ActivityDetailStatusLabel, ActivityDetailStatus,
                    ActivityDetailEncodedByLabel, ActivityDetailEncodedBy
                };

                Paragraph ActivityDetailDetailColumn2 = new Paragraph
                {
                    ActivityDetailACNumberLabel, ActivityDetailACNumber,
                    ActivityDetailActivityLabel, ActivityDetailActivity,
                    ActivityDetailTransportationFeeLabel, ActivityDetailOnSiteCost,
                    ActivityDetailOnSiteCostLabel, ActivityDetailOnSiteCost,
                    ActivityDetailTotalCostLabel, ActivityDetailTotalCost
                };

                tableLead.AddCell(new PdfPCell(ActivityDetailDetailColumn1) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                tableLead.AddCell(new PdfPCell(ActivityDetailDetailColumn2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });


                PdfPTable tableLeadActivity = new PdfPTable(2);
                tableLeadActivity.SetWidths(new float[] { 80f, 80f });
                tableLeadActivity.WidthPercentage = 100;

                Phrase tableLeadActivitySignatureOverPrinterName = new Phrase("_________________________________ \nSignature over printed name", fontArial12);
                Phrase tableLeadActivityDate = new Phrase("_________________________________ \nDate", fontArial12);

                Paragraph tableLeadActivitySignatureOverPrinterNameParagraph = new Paragraph
                {
                    tableLeadActivitySignatureOverPrinterName
                };

                Paragraph tableLeadActivityDateParagraph = new Paragraph
                {
                    tableLeadActivityDate
                };

                tableLeadActivity.AddCell(new PdfPCell(tableLeadActivitySignatureOverPrinterNameParagraph) { PaddingTop = 15f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f, Border = 0 });
                tableLeadActivity.AddCell(new PdfPCell(tableLeadActivityDateParagraph) { PaddingTop = 15f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f, Border = 0 });

                PdfPCell colspan2 = new PdfPCell(tableLeadActivity);
                colspan2.Colspan = 2;
                colspan2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableLead.AddCell(new PdfPCell(colspan2) { PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(tableLead);
            }

            // ==============
            // Close Document
            // ==============
            document.Close();

            // ===============
            // Response Stream
            // ===============
            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=customer.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        [HttpGet, Route("salesdelivery/activity/{id}")]
        public HttpResponseMessage PdfSalesDeliveryActivity(string id)
        {
            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 40f, 30f, 40f);

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { PaddingTop = 5f, Border = 0 });

            // =============
            // Open Document
            // =============
            document.Open();

            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User not found!");
            }

            var salesDeliveryActivity = from d in db.CrmTrnActivities
                               where d.Id == Convert.ToInt32(id)
                               select d;

            if (salesDeliveryActivity.Any())
            {

                var company = from d in db.MstCompanies
                              where d.Id == currentUser.FirstOrDefault().CompanyId
                              select d;

                // =========
                // Lead Data
                // =========
                String CompanyName = company.FirstOrDefault().Company;
                String SDNumber = salesDeliveryActivity.FirstOrDefault().CrmTrnSalesDelivery.SDNumber;
                String Date = salesDeliveryActivity.FirstOrDefault().CrmTrnSalesDelivery.SDDate.ToShortDateString();
                String Name = salesDeliveryActivity.FirstOrDefault().CrmTrnSalesDelivery.MstArticle.Article;
                String Product = salesDeliveryActivity.FirstOrDefault().CrmTrnSalesDelivery.CrmMstProduct.ProductCode;
                String FunctionalStaff = salesDeliveryActivity.FirstOrDefault().MstUser1.FullName;
                String TechnicalStaff = salesDeliveryActivity.FirstOrDefault().MstUser2.FullName;
                String DateStart = salesDeliveryActivity.FirstOrDefault().StartDateTime.ToShortDateString();
                String DateEnd = salesDeliveryActivity.FirstOrDefault().EndDateTime.ToShortDateString();
                String Status = salesDeliveryActivity.FirstOrDefault().CrmTrnSalesDelivery.Status;
                String EncodedBy = salesDeliveryActivity.FirstOrDefault().MstUser3.FullName;
                String ACNumber = salesDeliveryActivity.FirstOrDefault().ACNumber;
                String Activity = salesDeliveryActivity.FirstOrDefault().Activity;
                String TransportationFee = salesDeliveryActivity.FirstOrDefault().TransportationCost.ToString("#,##0.00");
                String OnSiteCost = salesDeliveryActivity.FirstOrDefault().OnSiteCost.ToString("#,##0.00");
                Decimal totalCost = salesDeliveryActivity.FirstOrDefault().TransportationCost + salesDeliveryActivity.FirstOrDefault().OnSiteCost;
                String totalCostString = totalCost.ToString("#,##0.00");

                PdfPTable tableLead = new PdfPTable(2);
                tableLead.SetWidths(new float[] { 80f, 80f });
                tableLead.WidthPercentage = 100;

                PdfPCell colspan1 = new PdfPCell(new Phrase("Sales Delivery Transaction \n" + CompanyName));

                colspan1.Colspan = 2;
                colspan1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableLead.AddCell(colspan1);

                Phrase ActivityDetailLDNumberLabel = new Phrase("SD No: ", fontArial12);
                Phrase ActivityDetailLDNumber = new Phrase(SDNumber + "\n", fontArial12Bold);

                Phrase ActivityDetailDateLabel = new Phrase("SD Date: ", fontArial12);
                Phrase ActivityDetailDate = new Phrase(Date + "\n", fontArial12Bold);

                Phrase ActivityDetailNameLabel = new Phrase("Lead's Name: ", fontArial12);
                Phrase ActivityDetailName = new Phrase(Name + "\n", fontArial12Bold);

                Phrase ActivityDetailProductlabel = new Phrase("Product: ", fontArial12);
                Phrase ActivityDetailProduct = new Phrase(Product + "\n", fontArial12Bold);

                Phrase ActivityDetailFunctionalStaffLabel = new Phrase("Functional Staff: ", fontArial12);
                Phrase ActivityDetailFunctionalStaff = new Phrase(FunctionalStaff + "\n", fontArial12Bold);

                Phrase ActivityDetailTechnicalStaffLabel = new Phrase("Technical Staff: ", fontArial12);
                Phrase ActivityDetailTechnicalStaff = new Phrase(TechnicalStaff + "\n", fontArial12Bold);

                Phrase ActivityDetailDateStartrLabel = new Phrase("Date Start: ", fontArial12);
                Phrase ActivityDetailDateStart = new Phrase(DateStart + "\n", fontArial12Bold);

                Phrase ActivityDetailDateEndLabel = new Phrase("Date End: ", fontArial12);
                Phrase ActivityDetailDateEnd = new Phrase(DateEnd + "\n", fontArial12Bold);

                Phrase ActivityDetailStatusLabel = new Phrase("Status: ", fontArial12);
                Phrase ActivityDetailStatus = new Phrase(Status + "\n", fontArial12Bold);

                Phrase ActivityDetailEncodedByLabel = new Phrase("EncodedBy: ", fontArial12);
                Phrase ActivityDetailEncodedBy = new Phrase(EncodedBy + "\n", fontArial12Bold);

                Phrase ActivityDetailACNumberLabel = new Phrase("ACNumber: ", fontArial12);
                Phrase ActivityDetailACNumber = new Phrase(ACNumber + "\n", fontArial12Bold);

                Phrase ActivityDetailActivityLabel = new Phrase("Activity: ", fontArial12);
                Phrase ActivityDetailActivity = new Phrase(Activity + "\n", fontArial12Bold);

                Phrase ActivityDetailTransportationFeeLabel = new Phrase("Transportation Fee: ", fontArial12);
                Phrase ActivityDetailTransportationFee = new Phrase(TransportationFee + "\n", fontArial12Bold);

                Phrase ActivityDetailOnSiteCostLabel = new Phrase("On Site Cost: ", fontArial12);
                Phrase ActivityDetailOnSiteCost = new Phrase(OnSiteCost + "\n", fontArial12Bold);

                Phrase ActivityDetailTotalCostLabel = new Phrase("Total Cos: ", fontArial12);
                Phrase ActivityDetailTotalCost = new Phrase(totalCost.ToString("#,##0.00") + "\n", fontArial12Bold);

                Paragraph ActivityDetailDetailColumn1 = new Paragraph
                {
                    ActivityDetailLDNumberLabel, ActivityDetailLDNumber,
                    ActivityDetailDateLabel, ActivityDetailDate,
                    ActivityDetailNameLabel, ActivityDetailName,
                    ActivityDetailProductlabel, ActivityDetailProduct,
                    ActivityDetailFunctionalStaffLabel, ActivityDetailFunctionalStaff,
                    ActivityDetailTechnicalStaffLabel, ActivityDetailTechnicalStaff,
                    ActivityDetailDateStartrLabel, ActivityDetailDateStart,
                    ActivityDetailDateEndLabel, ActivityDetailDateEnd,
                    ActivityDetailStatusLabel, ActivityDetailStatus,
                    ActivityDetailEncodedByLabel, ActivityDetailEncodedBy
                };

                Paragraph ActivityDetailDetailColumn2 = new Paragraph
                {
                    ActivityDetailACNumberLabel, ActivityDetailACNumber,
                    ActivityDetailActivityLabel, ActivityDetailActivity,
                    ActivityDetailTransportationFeeLabel, ActivityDetailOnSiteCost,
                    ActivityDetailOnSiteCostLabel, ActivityDetailOnSiteCost,
                    ActivityDetailTotalCostLabel, ActivityDetailTotalCost
                };

                tableLead.AddCell(new PdfPCell(ActivityDetailDetailColumn1) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                tableLead.AddCell(new PdfPCell(ActivityDetailDetailColumn2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });


                PdfPTable tableLeadActivity = new PdfPTable(2);
                tableLeadActivity.SetWidths(new float[] { 80f, 80f });
                tableLeadActivity.WidthPercentage = 100;

                Phrase tableLeadActivitySignatureOverPrinterName = new Phrase("_________________________________ \nSignature over printed name", fontArial12);
                Phrase tableLeadActivityDate = new Phrase("_________________________________ \nDate", fontArial12);

                Paragraph tableLeadActivitySignatureOverPrinterNameParagraph = new Paragraph
                {
                    tableLeadActivitySignatureOverPrinterName
                };

                Paragraph tableLeadActivityDateParagraph = new Paragraph
                {
                    tableLeadActivityDate
                };

                tableLeadActivity.AddCell(new PdfPCell(tableLeadActivitySignatureOverPrinterNameParagraph) { PaddingTop = 15f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f, Border = 0 });
                tableLeadActivity.AddCell(new PdfPCell(tableLeadActivityDateParagraph) { PaddingTop = 15f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f, Border = 0 });

                PdfPCell colspan2 = new PdfPCell(tableLeadActivity);
                colspan2.Colspan = 2;
                colspan2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableLead.AddCell(new PdfPCell(colspan2) { PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(tableLead);
            }

            // ==============
            // Close Document
            // ==============
            document.Close();

            // ===============
            // Response Stream
            // ===============
            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=customer.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        [HttpGet, Route("support/activity/{id}")]
        public HttpResponseMessage PdfSupportliveryActivity(string id)
        {
            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 40f, 30f, 40f);

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { PaddingTop = 5f, Border = 0 });

            // =============
            // Open Document
            // =============
            document.Open();

            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;

            if (currentUser == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User not found!");
            }

            var supportActivity = from d in db.CrmTrnActivities
                               where d.Id == Convert.ToInt32(id)
                               select d;

            if (supportActivity.Any())
            {

                var company = from d in db.MstCompanies
                              where d.Id == currentUser.FirstOrDefault().CompanyId
                              select d;

                // =========
                // Lead Data
                // =========
                String CompanyName = company.FirstOrDefault().Company;
                String SPNumber = supportActivity.FirstOrDefault().CrmTrnSupport.SPNumber;
                String Date = supportActivity.FirstOrDefault().CrmTrnSupport.SPDate.ToShortDateString();
                String Name = supportActivity.FirstOrDefault().CrmTrnSupport.MstArticle.Article;
                String Product = "";
                String FunctionalStaff = supportActivity.FirstOrDefault().MstUser1.FullName;
                String TechnicalStaff = supportActivity.FirstOrDefault().MstUser2.FullName;
                String DateStart = supportActivity.FirstOrDefault().StartDateTime.ToShortDateString();
                String DateEnd = supportActivity.FirstOrDefault().EndDateTime.ToShortDateString();
                String Status = supportActivity.FirstOrDefault().CrmTrnSupport.Status;
                String EncodedBy = supportActivity.FirstOrDefault().MstUser3.FullName;
                String ACNumber = supportActivity.FirstOrDefault().ACNumber;
                String Activity = supportActivity.FirstOrDefault().Activity;
                String TransportationFee = supportActivity.FirstOrDefault().TransportationCost.ToString("#,##0.00");
                String OnSiteCost = supportActivity.FirstOrDefault().OnSiteCost.ToString("#,##0.00");
                Decimal totalCost = supportActivity.FirstOrDefault().TransportationCost + supportActivity.FirstOrDefault().OnSiteCost;
                String totalCostString = totalCost.ToString("#,##0.00");

                PdfPTable tableLead = new PdfPTable(2);
                tableLead.SetWidths(new float[] { 80f, 80f });
                tableLead.WidthPercentage = 100;

                PdfPCell colspan1 = new PdfPCell(new Phrase("Support Transaction \n" + CompanyName));

                colspan1.Colspan = 2;
                colspan1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableLead.AddCell(colspan1);

                Phrase ActivityDetailLDNumberLabel = new Phrase("SP No: ", fontArial12);
                Phrase ActivityDetailLDNumber = new Phrase(SPNumber + "\n", fontArial12Bold);

                Phrase ActivityDetailDateLabel = new Phrase("SP Date: ", fontArial12);
                Phrase ActivityDetailDate = new Phrase(Date + "\n", fontArial12Bold);

                Phrase ActivityDetailNameLabel = new Phrase("Support's Name: ", fontArial12);
                Phrase ActivityDetailName = new Phrase(Name + "\n", fontArial12Bold);

                Phrase ActivityDetailProductlabel = new Phrase("Product: ", fontArial12);
                Phrase ActivityDetailProduct = new Phrase(Product + "\n", fontArial12Bold);

                Phrase ActivityDetailFunctionalStaffLabel = new Phrase("Functional Staff: ", fontArial12);
                Phrase ActivityDetailFunctionalStaff = new Phrase(FunctionalStaff + "\n", fontArial12Bold);

                Phrase ActivityDetailTechnicalStaffLabel = new Phrase("Technical Staff: ", fontArial12);
                Phrase ActivityDetailTechnicalStaff = new Phrase(TechnicalStaff + "\n", fontArial12Bold);

                Phrase ActivityDetailDateStartrLabel = new Phrase("Date Start: ", fontArial12);
                Phrase ActivityDetailDateStart = new Phrase(DateStart + "\n", fontArial12Bold);

                Phrase ActivityDetailDateEndLabel = new Phrase("Date End: ", fontArial12);
                Phrase ActivityDetailDateEnd = new Phrase(DateEnd + "\n", fontArial12Bold);

                Phrase ActivityDetailStatusLabel = new Phrase("Status: ", fontArial12);
                Phrase ActivityDetailStatus = new Phrase(Status + "\n", fontArial12Bold);

                Phrase ActivityDetailEncodedByLabel = new Phrase("EncodedBy: ", fontArial12);
                Phrase ActivityDetailEncodedBy = new Phrase(EncodedBy + "\n", fontArial12Bold);

                Phrase ActivityDetailACNumberLabel = new Phrase("ACNumber: ", fontArial12);
                Phrase ActivityDetailACNumber = new Phrase(ACNumber + "\n", fontArial12Bold);

                Phrase ActivityDetailActivityLabel = new Phrase("Activity: ", fontArial12);
                Phrase ActivityDetailActivity = new Phrase(Activity + "\n", fontArial12Bold);

                Phrase ActivityDetailTransportationFeeLabel = new Phrase("Transportation Fee: ", fontArial12);
                Phrase ActivityDetailTransportationFee = new Phrase(TransportationFee + "\n", fontArial12Bold);

                Phrase ActivityDetailOnSiteCostLabel = new Phrase("On Site Cost: ", fontArial12);
                Phrase ActivityDetailOnSiteCost = new Phrase(OnSiteCost + "\n", fontArial12Bold);

                Phrase ActivityDetailTotalCostLabel = new Phrase("Total Cos: ", fontArial12);
                Phrase ActivityDetailTotalCost = new Phrase(totalCost.ToString("#,##0.00") + "\n", fontArial12Bold);

                Paragraph ActivityDetailDetailColumn1 = new Paragraph
                {
                    ActivityDetailLDNumberLabel, ActivityDetailLDNumber,
                    ActivityDetailDateLabel, ActivityDetailDate,
                    ActivityDetailNameLabel, ActivityDetailName,
                    ActivityDetailProductlabel, ActivityDetailProduct,
                    ActivityDetailFunctionalStaffLabel, ActivityDetailFunctionalStaff,
                    ActivityDetailTechnicalStaffLabel, ActivityDetailTechnicalStaff,
                    ActivityDetailDateStartrLabel, ActivityDetailDateStart,
                    ActivityDetailDateEndLabel, ActivityDetailDateEnd,
                    ActivityDetailStatusLabel, ActivityDetailStatus,
                    ActivityDetailEncodedByLabel, ActivityDetailEncodedBy
                };

                Paragraph ActivityDetailDetailColumn2 = new Paragraph
                {
                    ActivityDetailACNumberLabel, ActivityDetailACNumber,
                    ActivityDetailActivityLabel, ActivityDetailActivity,
                    ActivityDetailTransportationFeeLabel, ActivityDetailOnSiteCost,
                    ActivityDetailOnSiteCostLabel, ActivityDetailOnSiteCost,
                    ActivityDetailTotalCostLabel, ActivityDetailTotalCost
                };

                tableLead.AddCell(new PdfPCell(ActivityDetailDetailColumn1) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                tableLead.AddCell(new PdfPCell(ActivityDetailDetailColumn2) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });


                PdfPTable tableLeadActivity = new PdfPTable(2);
                tableLeadActivity.SetWidths(new float[] { 80f, 80f });
                tableLeadActivity.WidthPercentage = 100;

                Phrase tableLeadActivitySignatureOverPrinterName = new Phrase("_________________________________ \nSignature over printed name", fontArial12);
                Phrase tableLeadActivityDate = new Phrase("_________________________________ \nDate", fontArial12);

                Paragraph tableLeadActivitySignatureOverPrinterNameParagraph = new Paragraph
                {
                    tableLeadActivitySignatureOverPrinterName
                };

                Paragraph tableLeadActivityDateParagraph = new Paragraph
                {
                    tableLeadActivityDate
                };

                tableLeadActivity.AddCell(new PdfPCell(tableLeadActivitySignatureOverPrinterNameParagraph) { PaddingTop = 15f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f, Border = 0 });
                tableLeadActivity.AddCell(new PdfPCell(tableLeadActivityDateParagraph) { PaddingTop = 15f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f, Border = 0 });

                PdfPCell colspan2 = new PdfPCell(tableLeadActivity);
                colspan2.Colspan = 2;
                colspan2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                tableLead.AddCell(new PdfPCell(colspan2) { PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(tableLead);
            }

            // ==============
            // Close Document
            // ==============
            document.Close();

            // ===============
            // Response Stream
            // ===============
            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=customer.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }



    }
}
