var get_Url = '';
$(function () {
    $("#txtFromDate").datepicker();
    $("#txtToDate").datepicker();
    $("#Amount").attr("readonly", "readonly");
});

var changeFromYear = $("#FromDate").datepicker("option", "changeYear");
var changeToYear = $("#ToDate").datepicker("option", "changeYear");
// Setter
$("#FromDate").datepicker("option", "changeFromYear", true);
$("#ToDate").datepicker("option", "changeToYear", true);

$("#btnSearch").click(function () {
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
                        + '<th>CNNo</th><th>Weight</th><th>Departure Date</th><th> Amount</th><th>Action</th></tr><tr><td colspan="9">No Record found</td></tr>');
                    $("#lblTotal").text();
                    hideAjaxLoader();
                } else {
                    $("#SearchCourierlist").html(data.html);
                    $("#lblTotal").html(data.TotalRecord.TotalAmount);
                    $("#lblFullCahrges").html(data.TotalRecord.FullCharges);
                    $("#hdnIsIGSTParty").val(data?.PartyDetails?.IsIGST);
                    $("#lblNetTotal").html(data.TotalRecord.NetAmount);
                    $("#lblDiscount").html(data.TotalRecord.Discount);
                    var CGST = data.TotalRecord.CGST;
                    var SGST = data.TotalRecord.SGST;
                    if (data?.PartyDetails?.IsIGST) {
                        var totalIgst = (parseFloat(CGST) + parseFloat(SGST)).toFixed(2);
                        $('#lblBillIGST').text(totalIgst);
                        $('#trCGST').hide();
                        $('#trSGST').hide();
                        $('#trIGST').show();
                    } else {
                        $('#lblBillCGST').text(CGST);
                        $('#lblBillSGST').text(SGST);
                        $('#trIGST').hide();
                        $('#trCGST').show();
                        $('#trSGST').show();
                    }
                    $("#lblGrandTotal").html(data.TotalRecord.GrandTotal);
                    $("#hdnPartyName").val(data.PartyDetails.PartyName);
                    $("#hdnAddress").val(data.PartyDetails.Address);
                    $("#hdnRecordCount").val(data.TotalRecord.RecordCount);
                    $('#partyGSTNumber').text(data.PartyDetails.GSTNumber);
                    $('#hdnPartyGSTNo').val(data.PartyDetails.GSTNumber);
                    $('#lblGrandTotalInWord').text(data.InWord);
                    $('#partyFuelCharge').text(data.TotalRecord.FuelChargesLabel);
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

        alert("Selected from date is greater than to date")
    }
});

$("#btnClear").click(function () {
    hideAjaxLoader();
    $("#SearchCourierlist").html('<table class="table"><tr><th>Tracking No</th><th> Party Name</th><th>Source Name</th><th>Destination Name</th>'
        + '<th>CNNo</th><th>Weight</th><th>Departure Date</th><th> Amount</th><th>Action</th></tr><tr><td colspan="9">No Record found</td></tr>');
});


$("#btnExportPdf").click(function () {
    $.ajax({
        type: 'GET',
        datatype: 'json',
        url: '/CourierDetails/GetLoggedInUser',
        success: function (data) {
            var header = `<center>
                <div style="margin-top:-3%;margin-bottom:-3%">
                    <img src="${data?.loggedInUser?.CompanyLogo}" style="height:150px;" />
                </div>
                <h5>${data?.loggedInUser?.Address1}, ${data?.loggedInUser?.Address2}, ${data?.loggedInUser?.City}-${data?.loggedInUser?.Pincode}, ${data?.loggedInUser?.State}.</h5>
                <h5>Tel :+91${data?.loggedInUser?.MobileNo}${data?.loggedInUser?.AlternateContact ? '/+91' + data?.loggedInUser?.AlternateContact : ''} </h4><h4>Email Address: ${data?.loggedInUser?.EmailId}</h5 ><hr />
                <table style="width:100%;">
                <tr>
                <td>M/s</td>
                <td> ${$("#hdnPartyName").val()}</td>
                <td>G.S.T. No.</td>
                <td>${$('#hdnPartyGSTNo').val()}</td>
                </tr>
                <tr>
                <td>Address</td>
                <td colspan="3">${$('#hdnAddress').val()}</td>
                </tr>
                <tr>
                <td>From</td>
                <td>${$('#txtFromDate').val()}</td>
                <td>To</td>'
                <td>${$('#txtToDate').val()}</td>
                </tr>
                 </table>
                </center>`;
            var footer = `<table style='width:100%;'>
        <tr>
            <td><strong>Bank Name</strong></td>
            <td>${data?.loggedInUser?.BankName}</td>
            <td><strong>A/C</strong></td>
            <td>${data?.loggedInUser?.AccountNumber}</td>
        </tr>
        <tr>
            <td><strong>IFSC Code</strong></td>
            <td>${data?.loggedInUser?.IFSCCode}</td>
            <td><strong>PAN NO</strong></td>
            <td>${data?.loggedInUser?.PANNumber}</td>
        </tr>
        <tr>
            <td><strong>GST Number</strong></td>
            <td colspan="3">${data?.loggedInUser?.GSTNumber}</td>
        </tr>
    </table>`;

            $('.table-bordered tr').each(function () {
                $(this).children('th').eq(7).remove();
                $(this).children('td').eq(7).remove();
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
        },
        error: function (err) {
            console.log("error=", err);
        }
    });
});

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

function calculateAmount() {
    var weight = parseFloat($("#Weight").val() == '' ? 0 : $("#Weight").val());
    var ODACharges = parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val());
    var discount = parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val());
    var qty = parseInt($("#Qty").val() == '' ? 0 : $("#Qty").val());
    var rate = parseInt($("#Rate").val() == '' ? 0 : $("#Rate").val());
    if (weight > 0) {
        if (resp.length > 0) {
            var rateresp = resp.filter(n => n.FromWt <= weight && n.ToWt >= weight);
            if (rateresp.length > 0) {
                $("#Rate").val(rateresp[0].Rate);
                if (resp[0].PartyType == 'Logistic') {
                    if ($("#Qty").val() != "") {
                        var amt = rateresp[0].Rate * parseInt(qty)
                        $("#Amount").val(amt + ODACharges - discount);
                    }
                    else {
                        $("#Amount").val(rateresp[0].Rate + ODACharges - discount);
                    }

                }
                else {
                    $("#Amount").val(rateresp[0].Rate + ODACharges - discount);
                }

            }
            else {
                if (rate > 0) {
                    $("#Amount").val((rate * qty) + ODACharges - discount);
                }
                else {
                    $("#Amount").val('');
                }

            }
        } else {
            $("#Amount").val((rate * qty) + ODACharges - discount);
        }
    }
    else {
        //$("#Qty").val('');
        $("#Rate").val('');
        $("#Amount").val('');
    }

}


function ratechangedcalculateAmount() {
    var weight = parseFloat($("#Weight").val() == '' ? 0 : $("#Weight").val());
    var ODACharges = parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val());
    var discount = parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val());
    var qty = parseInt($("#Qty").val() == '' ? 0 : $("#Qty").val());
    var rate = parseInt($("#Rate").val() == '' ? 0 : $("#Rate").val());
    if (weight > 0) {
        if (resp.length > 0) {
            var rateresp = resp.filter(n => n.FromWt <= weight && n.ToWt >= weight);
            if (rateresp.length > 0) {
                if (resp[0].PartyType == 'Logistic') {
                    if ($("#Qty").val() != "") {
                        var amt = rate * parseInt(qty)
                        $("#Amount").val(amt + ODACharges - discount);
                        if (parseFloat($("#Amount").val()) <= 0) {
                            alert('please enter valid data(ODA Charges or Discount)')
                            $("#Amount").val('');
                            $("#ODACharges").val('');
                            $("#Discount").val('');
                            $("#Amount").val(amt + parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
                        }
                    }
                    else {
                        $("#Amount").val(rate + ODACharges - discount);
                        if (parseFloat($("#Amount").val()) <= 0) {
                            alert('please enter valid data(ODA Charges or Discount)')
                            $("#Amount").val('');
                            $("#ODACharges").val('');
                            $("#Discount").val('');
                            $("#Amount").val(amt + parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
                        }
                    }

                }
                else {
                    $("#Amount").val(rate + ODACharges - discount);
                    if (parseFloat($("#Amount").val()) <= 0) {
                        alert('please enter valid data(ODA Charges or Discount)')
                        $("#Amount").val('');
                        $("#ODACharges").val('');
                        $("#Discount").val('');
                        $("#Amount").val(amt + parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
                    }
                }

            }
            else {
                if (rate > 0) {
                    $("#Amount").val((rate * qty) + ODACharges - discount);
                    if (parseFloat($("#Amount").val()) <= 0) {
                        alert('please enter valid data(ODA Charges or Discount)')
                        $("#Amount").val('');
                        $("#ODACharges").val('');
                        $("#Discount").val('');
                        $("#Amount").val(amt + parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
                    }
                }
                else {
                    $("#Amount").val('');
                }

            }
        } else {
            $("#Amount").val((rate * qty) + ODACharges - discount);
            if (parseFloat($("#Amount").val()) <= 0) {
                alert('please enter valid data(ODA Charges or Discount)')
                $("#Amount").val('');
                $("#ODACharges").val('');
                $("#Discount").val('');
                $("#Amount").val(amt + parseInt($("#ODACharges").val() == '' ? 0 : $("#ODACharges").val()) - parseInt($("#Discount").val() == '' ? 0 : $("#Discount").val()));
            }
        }
    }
    else {
        //$("#Qty").val('');
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
                if (strResponse.Table[0].PartyType = 'Logistic') {
                    $("#Qty").attr("readonly", false);
                }
                else {
                    $("#Qty").attr("readonly", true);
                }
            }
            else {
                $("#Qty").attr("readonly", true);
                resp = [];
                alert("rate not available");
            }

        }
    })
}
