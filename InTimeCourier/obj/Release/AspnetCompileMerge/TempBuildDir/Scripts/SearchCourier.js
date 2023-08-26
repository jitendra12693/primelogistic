var get_Url = '';
var CGST = '';
var SGST = '';
var IGST = '';
var IGSTParty = false;
var NONGSTParty = false;
var courrierIdList = [];
var cridlist = [];
$(function () {
    $("#txtFromDate").datepicker();
   // $("#txtInvDate").datepicker();
    $("#txtToDate").datepicker();
    $("#Amount").attr("readonly", "readonly");
    $("#txtFromDate").attr("readonly", "readonly");
    $("#txtToDate").attr("readonly", "readonly");
});

var changeFromYear = $("#FromDate").datepicker("option", "changeYear");
var changeToYear = $("#ToDate").datepicker("option", "changeYear");
// Setter
$("#FromDate").datepicker("option", "changeFromYear", true);
$("#ToDate").datepicker("option", "changeToYear", true);
//$("#txtInvDate").datepicker("option", "changeToYear", true);

$("#btnSearch").click(function () {
    debugger;
    //$('#btnInvoice').hide();
    courrierIdList = [];
    IGSTParty = false;
    var partyId = $("#PartyId").val();
    //var fromDate = $("#txtFromDate").val();
    //var toDate = $("#txtToDate").val();
    var trackingNo = $('#TrackingNo').val();
    var fromDate = new Date($("#txtFromDate").val()); //Year, Month, Date

    var toDate = new Date($("#txtToDate").val()); //Year, Month, Date
    //if (dateOne > dateTwo) {
    //    alert("Date One is greather then Date Two.");
    //} else {
    //    alert("Date Two is greather then Date One.");
    //}
    //if (trackingNo =='' && partyId == '') {
    //    alert("Please select party name");
    //    return false;
    //}
    if (trackingNo == '' && (fromDate == '' || toDate == '')) {
        alert("Please select From and To date");
        return false;
    }

    if (fromDate == toDate || fromDate < toDate) {
        showAjaxLoader();
        $.ajax({
            type: 'GET',
            dataType: 'json',
            url: '/CourierDetails/Search',
            data: { partyId: partyId, trackingNo: trackingNo, fromDate: $("#txtFromDate").val(), toDate: $("#txtToDate").val() },
            success: function (data) {
                if (data.TotalRecord.RecordCount == 0) {
                    $("#SearchCourierlist").html('<table class="table"><tr><th>Tracking No</th><th> Party Name</th><th>Source Name</th><th>Destination Name</th>'
                        + '<th>AWBNo.</th><th>Weight</th><th>Departure Date</th><th> Amount</th><th>Action</th></tr><tr><td colspan="9">No Record found</td></tr>');
                    $("#lblTotal").text();
                    hideAjaxLoader();
                } else {
                    $("#SearchCourierlist").html(data.html);
                    $("#lblTotal").html(data.TotalRecord.TotalAmount.toFixed(2));
                    if (data.TotalRecord.FuelChargesLabel == "0.00 %") {
                        $("#lblFullCahrges").html('0.00');
                    }
                    else
                    {
                        $("#lblFullCahrges").html(data.TotalRecord.FullCharges.toFixed(2));
                    }
                    IGSTParty = data?.PartyDetails?.IsIGST;
                    $("#hdnIsIGSTParty").val(data?.PartyDetails?.IsIGST);
                    $("#lblNetTotal").html(data.TotalRecord.NetAmount.toFixed(2));
                    $("#lblDiscount").html(data.TotalRecord.Discount.toFixed(2));
                    CGST = parseFloat(data.TotalRecord.CGST.toFixed(2));
                    SGST = parseFloat(data.TotalRecord.SGST.toFixed(2));
                    IGST = (parseFloat(CGST) + parseFloat(SGST)).toFixed(2);
                    var html = '';
                    if (data?.PartyDetails?.IsIGST) {
                        $('#allcbIGST').prop('checked', true);
                        var totalIgst = (parseFloat(CGST) + parseFloat(SGST)).toFixed(2);
                        $('#lblBillIGST').text(totalIgst);
                        html = `
                                <span>I.G.S.T. (18 %)&nbsp;</span>
                               `
                        $('#gstValue').html('<b>'+totalIgst+'</b>');
                        $('#gstname').html(html);
                        $('#lblBillCGST').text('0.00');
                        $('#lblBillSGST').text('0.00');
                    } else {
                        $('#allcbGST').prop('checked', true);
                        $('#lblBillCGST').text(CGST);
                        $('#lblBillSGST').text(SGST);
                        html = `
                                <span>S.G.S.T. (9 %)</span>&nbsp;<hr/>
                                <span>C.G.S.T. (9 %)</span>&nbsp;
                               `
                        $('#gstValue').html('<b>'+CGST+'</b>' + '<hr/><b>' + SGST+'</b>');
                        $('#gstname').html(html);
                        $('#lblBillIGST').text('0.00');
                    }
                    var grndttl = 0;
                    var chkvaluetype = checkNumber(parseFloat(data.TotalRecord.GrandTotal.toFixed(2)));
                    if (chkvaluetype == 1) {
                        grndttl = data.TotalRecord.GrandTotal.toFixed(2);
                    }
                    else if (chkvaluetype == 2) {
                        grndttl = data.TotalRecord.GrandTotal.toFixed(2).split('.')
                        grndttl = (parseInt(grndttl[0]) + 1).toFixed(2);
                    }
                    $("#lblGrandTotal").html(grndttl);
                    $("#hdnPartyName").val(data.PartyDetails.PartyName);
                    $("#hdnAddress").val(data.PartyDetails.Address);
                    $("#hdnRecordCount").val(data.TotalRecord.RecordCount);
                    $('#partyGSTNumber').text(data.PartyDetails.GSTNumber);
                    $('#hdnPartyGSTNo').val(data.PartyDetails.GSTNumber);
                    $('#lblGrandTotalInWord').text(data.InWord);
                    $('#partyFuelCharge').text(data.TotalRecord.FuelChargesLabel);
                    $('#btnGenerateInvoice').show();
                    $('#chkall').show();
                    $('#allcb').prop('checked', true);
                    hideAjaxLoader();

                }
            },
            error: function (error) {
                alert('Some error occured ');
                hideAjaxLoader();
            }
        })
    }
    else {

        alert("Selected from date is greater than to date2")
    }
});

function checkNumber(x) {

    // check if the passed value is a number
    if (typeof x == 'number' && !isNaN(x)) {

        // check if it is integer
        if (Number.isInteger(x)) {
            return 1;
        }
        else {
            return 2
        }

    } 
}

$("#btnClear").click(function () {
    hideAjaxLoader();
    $("#SearchCourierlist").html('<table class="table"><tr><th>Tracking No</th><th> Party Name</th><th>Source Name</th><th>Destination Name</th>'
        + '<th>CNNo</th><th>Weight</th><th>Departure Date</th><th> Amount</th><th>Action</th></tr><tr><td colspan="9">No Record found</td></tr>');
   // $('#btnInvoice').hide();
});



function GenerateInvoice() {
    //$('#btnInvoice').hide();
    //---------------------------------------------------------------------------
    var partyId = $("#PartyId").val();
    var fromDate = $("#txtFromDate").val();
    var toDate = $("#txtToDate").val();
    if (partyId == '') {
        alert("Please select party name");
        return false;
    }
    if (fromDate == '' || toDate == '') {
        alert("Please select From and To date");
        return false;
    }
    var count = $('#hdnRecordCount').val();
    var partyId = $('#PartyId').val();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var grandTotal = $('#lblGrandTotal').text();
    var fullCharges = $('#lblFullCahrges').text();
    var TotalAmount = $('#lblTotal').text();
    var netAmount = $('#lblNetTotal').text();
    var discount = $('#lblDiscount').text();
    var InvoiceNo = $('#hdnInvoiceNumber').val();
    var SrNo = $('#hdnSrNo').val();
    var InvoiceDate = $('#hdnInvoiceDt').val();
    var gstType = $('input[name="GSTselector"]:checked').val();
    var addressType = $('input[name="Addselector"]:checked').val();
    if (count == '') {
        alert('Please first search your transaction');
        return false;
    }
    courrierIdList = [];
    $('tbody tr td input[type="checkbox"]').each(function () {
        if ($(this).prop('checked') == true) {
            courrierIdList.push($(this).val());
        }
    });
    if (courrierIdList.length > 0) {
        showAjaxLoader();
        $.ajax({
            type: 'POST',
            datatype: 'json',
            url: '/CourierDetails/SaveBillDetails',
            data: {
                partyId: partyId, fromDate: fromDate, toDate: toDate, grandTotal: grandTotal,
                TotalAmount: TotalAmount, fullCharges: fullCharges,
                CGST: CGST, SGST: SGST, InvoiceNo: InvoiceNo, SrNo: SrNo, InvoiceDate: InvoiceDate, courrierIdList: courrierIdList,
                gstType: gstType, addressType: addressType
            },
            success: function (data) {
                /*theDialog.dialog('open');*/
                hideAjaxLoader();
                //secajax(data.BillId, data.CurrentDate);
                if (data.billid != '' && data.CurrentDate != '') {
                    alert('Invoice Generated Successfully');
                    $('#btnGenerateInvoice').hide();
                    $('#chkall').hide();
                    $("#btnClear").click();
                    $('#hdnRecordCount').val('');
                    $('#hdnInvoiceDt').val('');
                    $('#hdnSrNo').val('');
                    $('#hdnInvoiceNumber').val('');
                    courrierIdList = [];
                }

            },
            error: function (error) {
                alert(error.responseText);
                hideAjaxLoader();
                //$('#btnInvoice').hide();
            }
        });
    }
    else {
        alert('Please Select atleast one AWB Entry')
    }
  
    //---------------------------------------------------------------------------
    }

function secajax(billid, billdate) {
    $('#btnGenerateInvoice').show();
    $('#chkall').hide();
    $('#hdnbillno').val(billid);
    $('#hdnbilldate').val(billdate);
    var datearray = $('#txtFromDate').val().split("/");
    var newdatefrom = datearray[2] + '/' + datearray[0] + '/' + datearray[1];
    newdatefrom = datearray[1] + '-' + new Date(newdatefrom).toLocaleString("en-us", { month: "short" }) + '-' + datearray[2]
    var datearray1 = $('#txtToDate').val().split("/");
    var newdateto = datearray1[2] + '/' + datearray1[0] + '/' + datearray1[1];
    newdateto = datearray1[1] + '-' + new Date(newdateto).toLocaleString("en-us", { month: "short" }) + '-' + datearray1[2]
    $.ajax({
        type: 'GET',
        datatype: 'json',
        url: '/CourierDetails/GetLoggedInUser',
        success: function (data) {
            var header = `<center>
                <div style="margin-top:-10px;margin-bottom:-30px">
                   <div><b> TAX INVOICE </b></div>
                    <hr style='margin-bottom:2%'>
                    <img src="${data?.loggedInUser?.CompanyLogo}" style="height:45px;float:left;margin-top:-1%" />
                </div>
             
                <div style='float:right'>
                <span style='font-size:small'>${data?.loggedInUser?.Address1}, ${data?.loggedInUser?.Address2}, ${data?.loggedInUser?.City}-${data?.loggedInUser?.Pincode}, ${data?.loggedInUser?.State}.</span>
                <span style='font-size:small'><br/>Tel :+91${data?.loggedInUser?.MobileNo}${data?.loggedInUser?.AlternateContact ? '/+91' + data?.loggedInUser?.AlternateContact : ''} </span>
                <span style='font-size:small'>, Email Address: ${data?.loggedInUser?.EmailId}</span>
                </div>
         
                <table style="width:100%;font-size:13px;margin-top:12%">

        <tr>
			<td colspan="2" style='text-align:left'><b>Bill To</b></td>
			<td style='text-align:right;width:20%'><b>Invoice No.&nbsp;</b></td>
			<td style='width:15%;text-align:center'><b>${$('#hdnbillno').val()}</b></td>
		</tr>
		<tr>
			<td colspan="2" rowspan="3" style='text-align:left'><b><u>${$("#hdnPartyName").val()}</u></b><br/>${$('#hdnAddress').val()}<br/><b>G.S.T. No. :- </b>${$('#hdnPartyGSTNo').val()}</td>
			<td style='text-align:right;width:20%'><b>Invoice Date &nbsp;</b></td>
			<td style='width:15%;text-align:center'><b>${$('#hdnbilldate').val()}</b></td>
		</tr>
		<tr>
			<td style='text-align:right;width:20%'><b>Period From &nbsp;</b></td>
			<td style='width:15%;text-align:center'><b>${newdatefrom}</b></td>
		</tr>
		<tr>
			<td style='text-align:right;width:20%'><b>Period To &nbsp;</b></td>
			<td style='width:15%;text-align:center'><b>${newdateto}</b></td>
		</tr>
                 </table>
                </center>`;
            var footer = `<table style='width:100%;font-size:13px;'>
       
         <tr>
			 <td style='width:65%;text-align:center'><strong>Terms & Conditions</strong></td>
			<td style='text-align:center'><strong>For Intime Logistic</strong></td>
		</tr>
		<tr>
			<td><p style='font-size:x-small'>
                    1. Kindly notify us in writing regarding any discrepancy in this invoice within 7 days. Otherwise, <br />This invoice shall be deemed to be correct & payable by you.
                    <br />2. All cheques should be drawn Cross 'A/c Payee' in favour of INTIME LOGISTICS .
                    <br />
                    3. Interest @2% per month will be charged on delayed payments.
                    <br />
                    4. All Invoices are to be Paid within the Credit Period Specified as per the Contractual Terms.
                    <br/>
                     5. Subject to Mumbai Jurisdiction
                </p></td>
			<td style='text-align:center'> <img style='height:50px' src="${data?.loggedInUser?.SignPic}"/></td>
		</tr>
		<tr>
            <td style='text-align:center'><strong>“This is Computer Generated Invoice”</strong></td>
			<td style='text-align:center'><strong>Authorised Signatory</strong></td>
		</tr>

    </table>`;
            
            debugger;
            $('.table-bordered tr').each(function () {
                $(this).children('th').eq(8).remove();
                $(this).children('td').eq(8).remove();
                $(this).children('th').eq(4).attr("colspan", "2");
                $(this).children('td').eq(4).attr("colspan", "2");
                //$('#myTable').attr("class","tableClass");
            });

            var divContents = document.getElementById("SearchCourierlist").innerHTML;
            var printWindow = window.open('', '', 'height=200,width=400');
            printWindow.document.write('<html><head><title></title><style>table, th, td {border: 1px solid black;border-collapse: collapse;}</style>');
            printWindow.document.write('</head><body >' + header);
            printWindow.document.write(divContents);
            printWindow.document.write(footer + '</body></html>');
            printWindow.document.close();
            printWindow.print();
            //$("#btnClear").click();
            //$('#hdnRecordCount').val('');
            //$('#hdnInvoiceDt').val('');
            //$('#hdnSrNo').val('');
            //$('#hdnInvoiceNumber').val('');
            //$('#btnInvoice').hide();
        },
        error: function (err) {
            console.log("error=", err);
            //$('#btnInvoice').hide();
        }
    });
}

function StringToInt(number) {
    if (number == null || number == undefined || number == NaN || number == '') {
        return 0;
    } else {
        return parseInt(number);
    }
}

$("#recieptModal").dialog({
    modal: true,
    autoOpen: false,
    title: "Reciept Details",
    width: 1000,
    height: 500
});
var theDialog = $("#recieptModal");

$("#btnReciept").click(function () {

    var partyId = $("#PartyId").val();
    var fromDate = $("#txtFromDate").val();
    var toDate = $("#txtToDate").val();
    if (partyId == '') {
        alert("Please select party name");
        return false;
    }
    if (fromDate == '' || toDate == '') {
        alert("Please select From and To date");
        return false;
    }
    var count = $('#hdnRecordCount').val();
    var partyId = $('#PartyId').val();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var grandTotal = $('#lblGrandTotal').text();
    var fullCharges = $('#lblFullCahrges').text();
    var TotalAmount = $('#lblTotal').text();
    var netAmount = $('#lblNetTotal').text();
    var discount = $('#lblDiscount').text();
    var CGST = $('#lblBillCGST').text() ? $('#lblBillCGST').text() : parseFloat($('#lblBillIGST').text()) / 2;
    var SGST = $('#lblBillSGST').text() ? $('#lblBillSGST').text() : parseFloat($('#lblBillIGST').text()) / 2;
    var billId = 0;//StringToInt($('#lblBillNo').text());
    var gstType = $('input[name="GSTselector"]:checked').val();
    var addressType = $('input[name="Addselector"]:checked').val();

    if (count == '') {
        alert('Please first search your transaction');
        return false;
    }
    showAjaxLoader();
    $.ajax({
        type: 'POST',
        datatype: 'json',
        url: '/CourierDetails/SaveBillDetails',
        data: {
            partyId: partyId, fromDate: fromDate, toDate: toDate, grandTotal: grandTotal,
            TotalAmount: TotalAmount, fullCharges: fullCharges,
            CGST: CGST, SGST: SGST, BillId: billId
        },
        success: function (data) {
            theDialog.dialog('open');
            $('#lblBillNo').text(data.BillId);
            $('#lblBillDate').text(data.CurrentDate);
            $('#lblPartyName').text($('#hdnPartyName').val());
            $('#lblAddress').text($('#hdnAddress').val());
            $('#lblFromDate').text(fromDate);
            $('#lblToDate').text(toDate);
            $('#lblBillAmount').text(netAmount);
            $('#lblBillTotal').text(TotalAmount);
            $('#lblBillNetTotal').text(netAmount);
            $('#lblBillDiscount').text(discount);
            $('#lblBillFullCahrges').text(fullCharges);
            if (get_Url == '1') {
                if ($('#hdnIsIGSTParty').val() == "true") {
                    var totalIgst = (parseFloat(CGST) + parseFloat(SGST)).toFixed(2);
                    $('#lblBillIGST_N').text(totalIgst);
                    $('#trBillCGST_N').hide();
                    $('#trBillSGST_N').hide();
                    $('#trBillIGST_N').show();
                } else {
                    $('#lblBillCGST_N').text(CGST);
                    $('#lblBillSGST_N').text(SGST);
                    $('#trBillIGST_N').hide();
                    $('#trBillCGST_N').show();
                    $('#trBillSGST_N').show();
                }
            }
            else {
                if ($('#hdnIsIGSTParty').val() == "true") {
                    var totalIgst = (parseFloat(CGST) + parseFloat(SGST)).toFixed(2);
                    $('#lblIGST').text(totalIgst);
                    $('#trBillCGST').hide();
                    $('#trBillSGST').hide();
                    $('#trBillIGST').show();
                } else {
                    $('#lblCGST').text(CGST);
                    $('#lblSGST').text(SGST);
                    $('#trBillIGST').hide();
                    $('#trBillCGST').show();
                    $('#trBillSGST').show();
                }
            }

           
            $('#lblBillGrandTotal').text(grandTotal);
            $('#partyGSTNumber').text()
            $('#divDisplayWords').text($('#lblGrandTotalInWord').text());
            $('#lblPartyFuelCharges').text($('#partyFuelCharge').text());
            hideAjaxLoader();
        },
        error: function (error) {
            alert(error.responseText);
            hideAjaxLoader();
        }
    });
});

$('#btnPrintReciept').click(function () {
    $.ajax({
        type: 'GET',
        datatype: 'json',
        url: '/CourierDetails/GetLoggedInUser',
        success: function (data) {
            var divContents = document.getElementById("divReciept").innerHTML;
            var printWindow = window.open('', '', 'height=200,width=400');
            printWindow.document.write('<html><head><title></title><style>table, th, td {border: 1px solid black;border-collapse: collapse;}</style>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(divContents);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.print();
        },
        error: function (err) {
            console.log("error=", err);
        }
    });

})
//}

function showAjaxLoader() {
    //find ajax loader div tag
    var loaderDiv = $("#divAjaxLoader");
    if (loaderDiv.length === 0) {
        //create ajax loader div tag, if not present
        loaderDiv = $("<div />;")
            .attr("id", "divAjaxLoader")
            .css("position", "absolute")
            .css("display", "block")
            .css("z-index", "10000")
            .addClass("ajaxLoader");
        loaderDiv.appendTo("body");
    }

    //center ajax loader div tag in the browser window
    var doc = $(document);
    loaderDiv.css('top', (doc.height() - loaderDiv.height()) / 2);
    loaderDiv.css('left', (doc.width() - loaderDiv.width()) / 2);

    //show it
    loaderDiv.show();

}

function hideAjaxLoader() {
    //hide ajax loader div tag, if present
    $("#divAjaxLoader").hide();
    $("#divAjaxLoader").css("display", "none");
}

function onlyNumbers(evt) {
    var e = event || evt; // For trans-browser compatibility
    var charCode = e.which || e.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function NumToWord(inputNumber, id) {
    var str = new String(inputNumber)
    var splt = str.split("");
    var rev = splt.reverse();
    var once = ['Zero', ' One', ' Two', ' Three', ' Four', ' Five', ' Six', ' Seven', ' Eight', ' Nine'];
    var twos = ['Ten', ' Eleven', ' Twelve', ' Thirteen', ' Fourteen', ' Fifteen', ' Sixteen', ' Seventeen', ' Eighteen', ' Nineteen'];
    var tens = ['', 'Ten', ' Twenty', ' Thirty', ' Forty', ' Fifty', ' Sixty', ' Seventy', ' Eighty', ' Ninety'];

    numLength = rev.length;
    var word = new Array();

    var j = 0;

    for (i = 0; i < numLength; i++) {
        switch (i) {

            case 0:
                if ((rev[i] == 0) || (rev[i + 1] == 1)) {
                    word[j] = '';
                }
                else {
                    word[j] = '' + once[rev[i]];
                }
                word[j] = word[j];
                break;

            case 1:
                aboveTens();
                break;

            case 2:
                if (rev[i] == 0) {
                    word[j] = '';
                }
                else if ((rev[i - 1] == 0) || (rev[i - 2] == 0)) {
                    word[j] = once[rev[i]] + " Hundred ";
                }
                else {
                    word[j] = once[rev[i]] + " Hundred and";
                }
                break;

            case 3:
                if (rev[i] == 0 || rev[i + 1] == 1) {
                    word[j] = '';
                }
                else {
                    word[j] = once[rev[i]];
                }
                if ((rev[i + 1] != 0) || (rev[i] > 0)) {
                    word[j] = word[j] + " Thousand";
                }
                break;


            case 4:
                aboveTens();
                break;

            case 5:
                if ((rev[i] == 0) || (rev[i + 1] == 1)) {
                    word[j] = '';
                }
                else {
                    word[j] = once[rev[i]];
                }
                if (rev[i + 1] !== '0' || rev[i] > '0') {
                    word[j] = word[j] + " Lakh";
                }

                break;

            case 6:
                aboveTens();
                break;

            case 7:
                if ((rev[i] == 0) || (rev[i + 1] == 1)) {
                    word[j] = '';
                }
                else {
                    word[j] = once[rev[i]];
                }
                if (rev[i + 1] !== '0' || rev[i] > '0') {
                    word[j] = word[j] + " Crore";
                }
                break;

            case 8:
                aboveTens();
                break;

            //            This is optional. 

            //            case 9:
            //                if ((rev[i] == 0) || (rev[i + 1] == 1)) {
            //                    word[j] = '';
            //                }
            //                else {
            //                    word[j] = once[rev[i]];
            //                }
            //                if (rev[i + 1] !== '0' || rev[i] > '0') {
            //                    word[j] = word[j] + " Arab";
            //                }
            //                break;

            //            case 10:
            //                aboveTens();
            //                break;

            default: break;
        }
        j++;
    }

    function aboveTens() {
        if (rev[i] == 0) { word[j] = ''; }
        else if (rev[i] == 1) { word[j] = twos[rev[i - 1]]; }
        else { word[j] = tens[rev[i]]; }
    }

    word.reverse();
    var finalOutput = '';
    for (i = 0; i < numLength; i++) {
        finalOutput = finalOutput + word[i];
    }

    $("#lblGrandTotalInWord").text(finalOutput);
}

//function calculateAmount() {
//    var weight = parseFloat($("#Weight").val() == '' ? 0 : $("#Weight").val());
//    var ODACharges = parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val());
//    var discount = parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val());
//    var qty = parseInt($("#Qty").val() == '' ? 0 : $("#Qty").val());
//    var rate = parseInt($("#Rate").val() == '' ? 0 : $("#Rate").val());
//    if (weight > 0) {
//        if (resp.length > 0) {
//            var rateresp = resp.filter(n => n.FromWt <= weight && n.ToWt >= weight);
//            if (rateresp.length > 0) {
//                $("#Rate").val(rateresp[0].Rate);
//                //if (resp[0].PartyType == 'Logistic') {
//                if ($("#Qty").val() != "") {
//                    var amt = rateresp[0].Rate * parseInt(qty)
//                    $("#Amount").val(amt + ODACharges - discount);
//                }
//                else {
//                    $("#Amount").val(rateresp[0].Rate + ODACharges - discount);
//                }

//                //}
//                //else {
//                //    $("#Amount").val(rateresp[0].Rate + ODACharges - discount);
//                //}

//            }
//            else {
//                $("#Rate").val('');
//                //if (rate > 0) {
//                //    $("#Amount").val((rate * qty) + ODACharges - discount);
//                //}
//                //else {
//                $("#Amount").val('');
//                       // }

//            }
//        } else {
//            $("#Amount").val((rate * qty) + ODACharges - discount);
//        }
//    }
//    else {
//        //$("#Qty").val('');
//        $("#Rate").val('');
//        $("#Amount").val('');
//    }

//}
function calculateAmount() {
    var weight = parseFloat($("#Weight").val() == '' ? 0 : $("#Weight").val());
    var ODACharges = parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val());
    var discount = parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val());
    var qty = parseInt($("#Qty").val() == '' ? 0 : $("#Qty").val());
    var rate = parseInt($("#Rate").val() == '' ? 0 : $("#Rate").val());
    if (weight > 0) {
        if (parseInt(weight) > 0) {
            $("#Qty").attr('readonly', 'readonly');
            if (resp.length > 0) {
                var rateresp = resp.filter(n => n.FromWt <= weight && n.ToWt >= weight);
                if (rateresp.length > 0) {
                    $("#Rate").val(rateresp[0].Rate);
                    //if (resp[0].PartyType == 'Logistic') {
                    if ($("#Qty").val() != "") {
                        if (Number.isInteger(weight) == false) {
                            $("#Qty").val(parseInt(weight) + 1);
                            qty = parseInt(weight) + 1;
                        }
                        else {
                            $("#Qty").val(weight);
                            qty = weight;
                        }
                        var amt = rateresp[0].Rate * parseInt(qty)
                        $("#Amount").val(amt + ODACharges - discount);
                    }
                    else {
                        $("#Amount").val(rateresp[0].Rate + ODACharges - discount);
                    }

                    //}
                    //else {
                    //    $("#Amount").val(rateresp[0].Rate + ODACharges - discount);
                    //}

                }
                else {
                    $("#Rate").val('');
                    //if (rate > 0) {
                    //    $("#Amount").val((rate * qty) + ODACharges - discount);
                    //}
                    //else {
                    $("#Amount").val('');
                    // }

                }
            } else {
                $("#Amount").val((rate * qty) + ODACharges - discount);
            }

        }
        else {
            $('#Qty').removeAttr('readonly');
            //$("#Qty").val("1");
            if (resp.length > 0) {
                var rateresp = resp.filter(n => n.FromWt <= weight && n.ToWt >= weight);
                if (rateresp.length > 0) {
                    $("#Rate").val(rateresp[0].Rate);
                    if ($("#Qty").val() != "") {
                        var amt = rateresp[0].Rate * parseInt(qty)
                        $("#Amount").val(amt + ODACharges - discount);
                    }
                    else {
                        $("#Amount").val(rateresp[0].Rate + ODACharges - discount);
                    }
                }
                else {
                    $("#Rate").val('');
                    $("#Amount").val('');
                }
            } else {
                $("#Amount").val((rate * qty) + ODACharges - discount);
            }
        }

    }
    else {
        $('#Qty').removeAttr('readonly');
        $("#Qty").val('1');
        $("#Rate").val('');
        $("#Amount").val('');
    }

}




var resp = [];
function fetchRateDetails() {
    $.ajax({
        url: '/Source/FetchRateMapping?modeId=' + $('#CourrierModeId').val() + '&networkModeId=' + $('#NetworkModeId').val() + '&partyId=' + $('#PartyId').val(),
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        //data: JSON.stringify(courrierDetail),
        success: function (data) {
            var strResponse = JSON.parse(data);
            resp = [];
            if (strResponse.Table.length > 0) {
                for (var i = 0; i < strResponse.Table.length; i++) {
                    resp.push(strResponse.Table[i]);
                }
                //if (strResponse.Table[0].PartyType = 'Logistic') {
                //    $("#Qty").attr("readonly", false);
                //}
                //else {
                //    $("#Qty").attr("readonly", true);
                //}
            }
            else {
               // $("#Qty").attr("readonly", true);
                resp = [];
                alert("rate not available");
            }

        }
    })
}


//function ratechangedcalculateAmount() {
//    var weight = parseFloat($("#Weight").val() == '' ? 0 : $("#Weight").val());
//    var ODACharges = parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val());
//    var discount = parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val());
//    var qty = parseInt($("#Qty").val() == '' ? 0 : $("#Qty").val());
//    var rate = parseInt($("#Rate").val() == '' ? 0 : $("#Rate").val());
//    if (weight > 0) {
//        if (resp.length > 0) {
//            var rateresp = resp.filter(n => n.FromWt <= weight && n.ToWt >= weight);
//            if (rateresp.length > 0) {
//                //if (resp[0].PartyType == 'Logistic') {
//                //    if ($("#Qty").val() != "") {
//                //        var amt = rate * parseInt(qty)
//                //        $("#Amount").val(amt + ODACharges - discount);
//                //        if (parseFloat($("#Amount").val()) <= 0) {
//                //            alert('please enter valid data(ODA Charges or Discount)')
//                //            $("#Amount").val('');
//                //            $("#ODACharges").val('');
//                //            $("#Discount").val('');
//                //            $("#Amount").val(amt + parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
//                //        }
//                //    }
//                //    else {
//                //        $("#Amount").val(rate + ODACharges - discount);
//                //        if (parseFloat($("#Amount").val()) <= 0) {
//                //            alert('please enter valid data(ODA Charges or Discount)')
//                //            $("#Amount").val('');
//                //            $("#ODACharges").val('');
//                //            $("#Discount").val('');
//                //            $("#Amount").val(amt + parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
//                //        }
//                //    }

//                //}
//                //else {
//                    $("#Amount").val(rate + ODACharges - discount);
//                    if (parseFloat($("#Amount").val()) <= 0) {
//                        alert('please enter valid data(ODA Charges or Discount)')
//                        $("#Amount").val('');
//                        $("#ODACharges").val('');
//                        $("#Discount").val('');
//                        $("#Amount").val(amt + parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
//                    }
//                //}

//            }
//            else {
//                if (rate > 0) {
//                    $("#Amount").val((rate * qty) + ODACharges - discount);
//                    if (parseFloat($("#Amount").val()) <= 0) {
//                        alert('please enter valid data(ODA Charges or Discount)')
//                        $("#Amount").val('');
//                        $("#ODACharges").val('');
//                        $("#Discount").val('');
//                        $("#Amount").val(amt + parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
//                    }
//                }
//                else {
//                    $("#Amount").val('');
//                }

//            }
//        } else {
//            $("#Amount").val((rate * qty) + ODACharges - discount);
//            if (parseFloat($("#Amount").val()) <= 0) {
//                alert('please enter valid data(ODA Charges or Discount)')
//                $("#Amount").val('');
//                $("#ODACharges").val('');
//                $("#Discount").val('');
//                $("#Amount").val(amt + parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
//            }
//        }
//    }
//    else {
//        $("#Qty").val('');
//        $("#Rate").val('');
//        $("#Amount").val('');
//    }

//}
function ratechangedcalculateAmount() {
    var weight = parseFloat($("#Weight").val() == '' ? 0 : $("#Weight").val());
    var ODACharges = parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val());
    var discount = parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val());
    var qty = parseInt($("#Qty").val() == '' ? 0 : $("#Qty").val());
    var rate = parseInt($("#Rate").val() == '' ? 0 : $("#Rate").val());
    if (weight > 0) {
        if (parseInt(weight) > 0) {
            $("#Qty").attr('readonly', 'readonly');
            if (resp.length > 0) {
                var rateresp = resp.filter(n => n.FromWt <= weight && n.ToWt >= weight);
                if (rateresp.length > 0) {
                    $("#Amount").val(rate * qty + ODACharges - discount);
                    if (parseFloat($("#Amount").val()) <= 0) {
                        alert('please enter valid data(Extra Charges or Discount)')
                        $("#Amount").val('');
                        $("#ODACharges").val('');
                        $("#Discount").val('');
                        $("#Amount").val(parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
                    }
                }
                else {
                    if (rate > 0) {
                        $("#Amount").val((rate * qty) + ODACharges - discount);
                        if (parseFloat($("#Amount").val()) <= 0) {
                            alert('please enter valid data(Extra Charges or Discount)')
                            $("#Amount").val('');
                            $("#ODACharges").val('');
                            $("#Discount").val('');
                            $("#Amount").val(parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
                        }
                    }
                    else {
                        $("#Amount").val('');
                    }

                }
            } else {
                $("#Amount").val((rate * qty) + ODACharges - discount);
                if (parseFloat($("#Amount").val()) <= 0) {
                    alert('please enter valid data(Extra Charges or Discount)')
                    $("#Amount").val('');
                    $("#ODACharges").val('');
                    $("#Discount").val('');
                    $("#Amount").val(parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
                }
            }
        }
        else {
            $('#Qty').removeAttr('readonly');
            // $("#Qty").val("1");
            if (resp.length > 0) {
                var rateresp = resp.filter(n => n.FromWt <= weight && n.ToWt >= weight);
                if (rateresp.length > 0) {
                    $("#Amount").val(rate * qty + ODACharges - discount);
                    if (parseFloat($("#Amount").val()) <= 0) {
                        alert('please enter valid data(Extra Charges or Discount)')
                        $("#Amount").val('');
                        $("#ODACharges").val('');
                        $("#Discount").val('');
                        $("#Amount").val(parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
                    }
                }
                else {
                    if (rate > 0) {
                        $("#Amount").val((rate * qty) + ODACharges - discount);
                        if (parseFloat($("#Amount").val()) <= 0) {
                            alert('please enter valid data(Extra Charges or Discount)')
                            $("#Amount").val('');
                            $("#ODACharges").val('');
                            $("#Discount").val('');
                            $("#Amount").val(parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
                        }
                    }
                    else {
                        $("#Amount").val('');
                    }

                }
            } else {
                $("#Amount").val((rate * qty) + ODACharges - discount);
                if (parseFloat($("#Amount").val()) <= 0) {
                    alert('please enter valid data(Extra Charges or Discount)')
                    $("#Amount").val('');
                    $("#ODACharges").val('');
                    $("#Discount").val('');
                    $("#Amount").val(parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
                }
            }
        }
    }
    else {
        $('#Qty').removeAttr('readonly');
        $("#Qty").val("1");
        $("#Rate").val('');
        $("#Amount").val('');
    }

}

function Delete() {
    var id = $('#CourrierId').val()
    if ($('#isInvoiceDone').val() == 'True') {
        alert('Bill Generated');
    }
    else {
        if (confirm("Are you sure to delete this item")) {
            $.ajax({
                type: "GET",
                url: "/CourierDetails/Delete",
                data: { id: id },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.response == 'Deleted') {
                        $('#divUpdateCourrier').modal('hide');
                        alert("Entry Deleted Succussfully !")
                        $('#btnSearch').click();
                        //window.location.href = "/CourierDetails/CourrierList";
                    }
                },
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }
    }
};


//$(function () {
//    $("#Location").autocomplete({
//        source: function (request, response) {
//            var LocTextValue = $("#Location").val()
//            var filterList = DestinationList.filter(x => x.Name.toLowerCase().includes(LocTextValue.toLowerCase()));

//            if (!filterList.length) {
//                var result = [
//                    {
//                        label: 'No destination found',
//                        value: response.term
//                    }
//                ];
//                response(result);
//                $('#DestinationId').val('0');
//            }
//            else {
//                response($.map(filterList, function (filterList, id) {
//                    return {
//                        label: filterList.Name,
//                        value: filterList.Id
//                    };
//                }));
//            }

//        },
//        select: function (e, i) {
//            $('#Location').val(i.item.label);
//            $('#DestinationId').val(i.item.value);
//            return false;
//        },
//        minLength: 1
//    });
//});

//$('#Location').on('blur', function () {
//    var Dest_Id = $('#DestinationId').val();
//    if (Dest_Id == '') {
//        $('#Location').val('');
//    }
//});


//$(function () {
//    $("#Distance").autocomplete({
//        source: function (request, response) {
//            var PartyTextValue = $("#Distance").val()
//            var filterList = PartyList.filter(x => x.Name.toLowerCase().includes(PartyTextValue.toLowerCase()));
//            if (!filterList.length) {
//                var result = [
//                    {
//                        label: 'No party found',
//                        value: response.term
//                    }
//                ];
//                response(result);
//                $('#PartyId').val('');
//            }
//            else {
//                response($.map(filterList, function (filterList, id) {
//                    return {
//                        label: filterList.Name,
//                        value: filterList.Id
//                    };
//                }));
//            }
//        },
//        select: function (e, i) {
//            $('#Distance').val(i.item.label);
//            $('#PartyId').val(i.item.value);
//            fetchRateDetails();
//            return false;
//        },
//        minLength: 1
//    });
//});

//$('#Distance').on('blur', function () {
//    var Party_Id = $('#PartyId').val();
//    if (Party_Id == '') {
//        $('#Distance').val('');
//    }
//});


var Party = '';
var Destination = ''
var PartyList = [];
var DestinationList = [];



//$("#btnUpdate").click(function (e) {
//    e.preventDefault();
//    alert('1')
//    //Show loading display here
//    var form = $("#signupform");
//    $.ajax({
//        url: '@Url.Action("EditPopup")',
//        data: form.serialize(),
//        type: 'POST',
//        success: function (data) {
//            //Show popup
//            $("#popup").html(data);
//        }
//    });
//});


$('#allcb').change(function () {
    if ($(this).prop('checked')) {
        $('tbody tr td input[type="checkbox"]').each(function () {
            $(this).prop('checked', true);
            /*$('#spnchk').text('Deselect All');*/
        });
        chkbox();
    } else {
        $('tbody tr td input[type="checkbox"]').each(function () {
            $(this).prop('checked', false);
           /* $('#spnchk').text('Select All');*/
        });
        chkbox();
    }
});



function chkbox() {
    cridlist = [];
    var fuelpercntg = $('#partyFuelCharge').text();
    var cnt = 0;
        $('tbody tr td input[type="checkbox"]').each(function () {
            if ($(this).prop('checked') == true) {
                cridlist.push($(this).val());
                cnt++;
            }
            else {
                cnt++;
            }
        });
    if (cnt == cridlist.length) {
        $('#allcb').prop('checked', true);
    }
    else {
        $('#allcb').prop('checked', false);
    }
    if (cridlist.length > 0) {
        showAjaxLoader();
        $.ajax({
            type: 'POST',
            datatype: 'json',
            url: '/CourierDetails/CalculateInvoiceData',
            data: { cridlist: cridlist, fuelpercntg: fuelpercntg, NONGSTParty: NONGSTParty },
            success: function (data) {
                var strresp = JSON.parse(data);
                if (strresp.length > 0) {
                    $("#lblTotal").html(strresp[0].TotalAmount.toFixed(2));
                    $("#lblNetTotal").html(strresp[0].NetAmount.toFixed(2));
                    $("#lblDiscount").html(strresp[0].Discount.toFixed(2));
                    $("#lblGrandTotal").html(strresp[0].GrandTotal.toFixed(2));
                    $("#hdnRecordCount").val(strresp[0].RecordCount);
                    $('#lblGrandTotalInWord').html(strresp[0].InWord);
                    $("#lblFullCahrges").html(strresp[0].FuelCharges.toFixed(2));
                    CGST = parseFloat(strresp[0].CGST.toFixed(2));
                    SGST = parseFloat(strresp[0].SGST.toFixed(2));
                    IGST = (parseFloat(CGST) + parseFloat(SGST)).toFixed(2);
                   // $('#partyFuelCharge').text(data.TotalRecord.FuelChargesLabel);
                    var html = '';
                    if (IGSTParty ==true) {
                        var totalIgst = (parseFloat(CGST) + parseFloat(SGST)).toFixed(2);
                        $('#lblBillIGST').text(totalIgst);
                        html = `
                                <span>I.G.S.T. (18 %)&nbsp;</span>
                               `
                        $('#gstValue').html('<b>' + totalIgst + '</b>');
                        $('#gstname').html(html);
                        $('#lblBillCGST').text('0.00');
                        $('#lblBillSGST').text('0.00');
                    }
                    else if (NONGSTParty == true)
                    {
                        $('#lblBillIGST').text(0.00);
                        html = `
                                <span>GST(N/A)&nbsp;</span>
                               `
                        $('#gstValue').html('<b>0.00</b>');
                        $('#gstname').html(html);
                        $('#lblBillCGST').text('0.00');
                        $('#lblBillSGST').text('0.00');
                    }
                    else {

                        $('#lblBillCGST').text(CGST.toFixed(2));
                        $('#lblBillSGST').text(SGST.toFixed(2));
                        html = `
                                <span>S.G.S.T. (9 %)</span>&nbsp;<hr/>
                                <span>C.G.S.T. (9 %)</span>&nbsp;
                               `
                        $('#gstValue').html('<b>' + CGST.toFixed(2) + '</b>' + '<hr/><b>' + SGST.toFixed(2) + '</b>');
                        $('#gstname').html(html);
                        $('#lblBillIGST').text('0.00');
                    }
                }
                else {
                    alert('something went wrong');
                }
                hideAjaxLoader();
            },
            error: function (error) {
                alert(error.responseText);
                hideAjaxLoader();
                //$('#btnInvoice').hide();
            }
        });
    }
    else {
        $("#lblTotal").html('0.00');
        $("#lblNetTotal").html('0.00');
        $("#lblDiscount").html('0.00');
        $("#lblGrandTotal").html('0.00');
        $("#hdnRecordCount").val('0');
        $('#lblGrandTotalInWord').html('N/A');
        $('#lblBillIGST').html('0.00');
        $('#lblBillCGST').html('0.00');
        $('#lblBillSGST').html('0.00');
        $("#lblFullCahrges").html('0.00');
        $('#gstValue').html('<b>0.00</b>' + '<hr/><b>0.00</b>');
        //alert('Please Select atleast one AWB Entry')
        //$('#allcb').prop('checked', false);
    }
};



$('#allcbAdd1').change(function () {
    if ($(this).prop('checked')) {
        $('#bnk2').hide();
        $('#acn2').hide();
        $('#ifsc2').hide();
        $('#pann2').hide();
        $('#gst2').hide();

        $('#bnk1').show();
        $('#acn1').show();
        $('#ifsc1').show();
        $('#pann1').show();
        $('#gst1').show();
        chkbox()
        alert('Address 1 Selected Successfully');
    } else {
        $('#bnk1').hide();
        $('#acn1').hide();
        $('#ifsc1').hide();
        $('#pann1').hide();
        $('#gst1').hide();

        $('#bnk2').show();
        $('#acn2').show();
        $('#ifsc2').show();
        $('#pann2').show();
        $('#gst2').show();
        chkbox()
        alert('Address 2 Selected Successfully');
    }
});

$('#allcbADD2').change(function () {
    if ($(this).prop('checked')) {
        $('#bnk1').hide();
        $('#acn1').hide();
        $('#ifsc1').hide();
        $('#pann1').hide();
        $('#gst1').hide();

        $('#bnk2').show();
        $('#acn2').show();
        $('#ifsc2').show();
        $('#pann2').show();
        $('#gst2').show();
        chkbox()
        alert('Address 2 Selected Successfully');
    } else {
        $('#bnk2').hide();
        $('#acn2').hide();
        $('#ifsc2').hide();
        $('#pann2').hide();
        $('#gst2').hide();

        $('#bnk1').show();
        $('#acn1').show();
        $('#ifsc1').show();
        $('#pann1').show();
        $('#gst1').show();
        chkbox()
        alert('Address 1 Selected Successfully');
    }
});

$('#allcbGST').change(function () {
    IGSTParty = false;
    NONGSTParty = false;
    $('#lblBillCGST').text(CGST);
    $('#lblBillSGST').text(SGST);
    html = `
                                <span>S.G.S.T. (9 %)</span>&nbsp;<hr/>
                                <span>C.G.S.T. (9 %)</span>&nbsp;
                               `
    $('#gstValue').html('<b>' + CGST + '</b>' + '<hr/><b>' + SGST + '</b>');
    $('#gstname').html(html);
    $('#lblBillIGST').text('0.00');
    chkbox()
    alert('GST Selected Successfully');
});
$('#allcbIGST').change(function () {
    IGSTParty = true;
    NONGSTParty = false;
    $('#lblBillIGST').text(IGST);
    html = `
                                <span>I.G.S.T. (18 %)&nbsp;</span>
                               `
    $('#gstValue').html('<b>' + IGST + '</b>');
    $('#gstname').html(html);
    $('#lblBillCGST').text('0.00');
    $('#lblBillSGST').text('0.00');
    chkbox()
    alert('IGST Selected Successfully')
});
$('#allcbNONGST').change(function () {
    IGSTParty = false;
    NONGSTParty = true;
    $('#lblBillIGST').text(0.00);
    html = `
                                <span> GST(N/A)&nbsp;</span>
                               `
    $('#gstValue').html('<b>0.00</b>');
    $('#gstname').html(html);
    $('#lblBillCGST').text('0.00');
    $('#lblBillSGST').text('0.00');

    chkbox();
    alert('NON GST Selected Successfully');
});



