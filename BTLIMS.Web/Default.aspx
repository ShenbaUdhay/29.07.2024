<%@ Page Language="C#" AutoEventWireup="true" Inherits="Default" EnableViewState="false"
    ValidateRequest="false" CodeBehind="Default.aspx.cs" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.ExpressApp.Web.Templates" TagPrefix="cc3" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.ExpressApp.Web.Controls" TagPrefix="cc4" %>
<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Main Page</title>
    <meta http-equiv="Expires" content="0" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <style type="text/css">
        /*New menuitem design, Combine this code - MKS*/
        /*body {
            background-color: #FFF !important;
        }
    
        .dxgvHeader_XafTheme,.dxgvHeader_XafTheme table {
            color: #605c5c;           wid
        }

        .xafNav .dxtv-ndSel {
            color: white !important;
        }

        .xafNav .dxtv-nd {
            color: black;
        }

        .menuLinks_XafTheme .dxm-item a.dx > span {
            color: black;
        }*/

        .PLM .GroupHeader .Label
        {
            padding-left: 5px !important;
            font-size: 11.5px !important;
            color: blue !important;
        }

        .PLMTAB td.GroupContent
        {
            width: 12% !important;
        }

        .PLM .dxichSys
        {
           margin: 0px !important;      
        }

        .PLMCNTRL 
        {
            width: 140px !important;          
        }

        .PLMBCNTRL 
        {
            width: 230px !important;          
        }

        .PLMCHK .Caption 
        {
            width: 0px !important;          
        }

        .PLMCHK .dxichCellSys 
        {
            padding-left: 0px !important;
        }

        .JobFormat {
            margin-top: 8px;
        }

        .Footer {  
            display: none;  
        }  
        td.BodyBackColor {  
            height: 0!important;  
        }  

        .h1 {
            color: Black;
            font-size: 250%;
            font-family: Verdana, Geneva, sans-serif;
            font-weight: bold;
            margin: 0px 0px 0px 0px;
            padding: 0px;
        }
        .tabfieldaddremove {
            max-width: 32px;
            padding-right: 32px ;
            padding-left: 32px;
            max-height: 32px;
            max-width: 32px;
        }
        .Helpcenter{
           font-weight:bold;
           font-size: large;
        }
        .welcometoourhelpcenter
        {
            font-weight:bold;
            font-size :large;
        }
        .redCell {
            color: white !important;
            background-color: red !important;
        }

         .yellowCell {
            color: black !important;
            background-color: yellow !important;
        }     

        .dxm-item.accountItem.dxm-subMenu .dx-vam {
            padding-left: 10px;
        }

        .dxm-item.accountItem.dxm-subMenu .dxm-image.dx-vam {
            border-radius: 32px;
            -moz-border-radius: 32px;
            -webkit-border-radius: 32px;
            padding-right: 0px !important;
            padding-left: 0px !important;
            max-height: 32px;
            max-width: 32px;
        }

        body {
            /*background-color: #FFFAFA;*/
        }
    </style>
    <script type="text/javascript">
                         var isLeft,
                             isRight,
                             isUp,
                             isDown,
                             isEnter,
                             columnIndex,
                             rowIndex;

                            function OnGetRowValues(Value) {
                                var Test = Value[0];
                                    if (Test == true) {
                                        if (confirm('Do you want to delete old test data')) {
                                            var value = 'Delete|' + Value[1];
                                            RaiseXafCallback(globalCallbackControl, 'id', value, '', false);
                                        }
                                    }     
                            }

                           function comboresize() { 
                               var ABLWidth = document.querySelector('[id$="ctl01_SSRC_T0G0I2"]').offsetWidth - document.querySelector('[id$="ctl01_SSRC_T0G0I2_CMB"]').offsetWidth; 
                               var MLWidth = document.querySelector('[id$="ctl01_SSRC_T0G0I0"]').offsetWidth - document.querySelector('[id$="ctl01_SSRC_T0G0I0_CMB"]').offsetWidth;                                 
                               document.querySelector('[id$="ctl01_SSRC_T0G0I0_CMB"]').style.marginLeft = (ABLWidth - MLWidth) + "px"; //57
                               var Test = document.querySelector('[id$="ctl01_SSRC_T0G0I1_CMB"]');
                               if (Test != null)
                               {
                                   var TLWidth = document.querySelector('[id$="ctl01_SSRC_T0G0I1"]').offsetWidth - Test.offsetWidth;    
                                   Test.style.marginLeft = (ABLWidth - TLWidth) + "px"; //64
                               }
                           }

                            function OnSLFlowRateTimeChanged(s, e) {
                                var regex = /[0-9]|\./;
                               if (!regex.test(e.htmlEvent.key)) {
                                  e.htmlEvent.returnValue = false;
                                }
                            }
                            function OnResultNumericChanged(s, e) {                           
                                var regex = /[0-9]|\.|\+|\<|\>|\-/;
                                if (!regex.test(e.htmlEvent.key)) {
                                    e.htmlEvent.returnValue = false;
                                }
                            }
        //function OnPressureRangeChanged(s, e) {
        //    alert('entered');                         
        //                        var regex = /([0 - 9] + [.-] *) +/;
        //                        if (!regex.test(e.htmlEvent.key)) {
        //                            e.htmlEvent.returnValue = false;
        //                        }
        //                    }

                        function OnGetRowValuesJobid(value) {
                            var val = 'Registration|' + value;
                            RaiseXafCallback(globalCallbackControl, 'id', val, '', false);
                        }

                        function onResultMouseover(s) {
                            s.title = '';
                            if (s.className.toString().includes('redCell')) {
                                s.title = 'The result is off the calibration range!';
                            }
                            else if (s.className.toString().includes('yellowCell')) {
                                s.title = 'The result is equal to rptlimit';
                            }                        
                        }

                           function OnGetValuesOnCustomCallbackComplete(values) {  
                               if (values != "error") {
                                   console.log('sss');
                                   var name = Grid.GetEditor('ResultNumeric').name;
                                   if (name != null)
                                   {
                                       var grid = name.split('_');
                                       grid.pop();
                                       grid.push('DXDataRow');
                                       name = grid.join('_');
                                       var mainsplitval = values.split(';');
                                       for (var strsplit of mainsplitval) {
                                           var splitval = strsplit.split('|');
                                           Grid.batchEditApi.SetCellValue(splitval[1], 'Result', splitval[0]);
                                           if (splitval[2] == "redcell") {
                                               $('#' + name + splitval[1] + ' td[fieldName=Result]').removeClass('yellowCell');
                                               $('#' + name + splitval[1] + ' td[fieldName=Result]').addClass('redCell');
                                           }
                                           else if (splitval[2] == "yellowcell") {
                                               $('#' + name + splitval[1] + ' td[fieldName=Result]').removeClass('redCell');
                                               $('#' + name + splitval[1] + ' td[fieldName=Result]').addClass('yellowCell');
                                           }
                                           else {
                                               $('#' + name + splitval[1] + ' td[fieldName=Result]').removeClass('yellowCell');
                                               $('#' + name + splitval[1] + ' td[fieldName=Result]').removeClass('redCell');
                                           }
                                       }
                                   }                                   
                               }
                            window.stopProgress();  
                        }

                         if (window.history.replaceState) {
                             window.history.replaceState(null, null, window.location.href);
                          }   

                            function qcuirefresh() {
                                OnClick('LPcell', 'separatorImage', true);
                                if (localStorage.getItem("stat") === null) {
                                    localStorage.setItem('stat', 'closed');	
                                    QCType.SetWidth((screen.width / 100) * 18);
                                    QCBatchSequence.SetWidth((screen.width / 100) * 70);
                                    var newwidth = ((((screen.width / 100) * 90) - 286) / 4);
                                    QCBatchID.SetWidth(newwidth);
                                    Datecreated.SetWidth(newwidth);
                                    CreatedBy.SetWidth(newwidth);
                                    Matrix.SetWidth(newwidth);
                                    Test.SetWidth(newwidth);
                                    Method.SetWidth(newwidth);
                                    Roomtemp.SetWidth(newwidth);
                                    Humidity.SetWidth(newwidth);
                                    //Instrument.SetWidth(newwidth);
                                    Jobid.SetWidth(newwidth);
                                    Noruns.SetWidth(newwidth);
                                    window.onbeforeunload = null;
                                }
                                else {
                                    localStorage.removeItem('stat');
                                    var totusablescr = screen.width - (document.getElementById('separatorCell').offsetWidth + document.getElementById('LPcell').offsetWidth);
                                    QCType.SetWidth((totusablescr / 100) * 18);
                                    QCBatchSequence.SetWidth((totusablescr / 100) * 70);      
                                    var tottxtusablescr = (((screen.width - (document.getElementById('separatorCell').offsetWidth + document.getElementById('LPcell').offsetWidth)) / 100) * 90) - (286);
                                    var newwidth = tottxtusablescr / 4;
                                    QCBatchID.SetWidth(newwidth);
                                    Datecreated.SetWidth(newwidth);
                                    CreatedBy.SetWidth(newwidth);
                                    Matrix.SetWidth(newwidth);
                                    Test.SetWidth(newwidth);
                                    Method.SetWidth(newwidth);
                                    Roomtemp.SetWidth(newwidth);
                                    Humidity.SetWidth(newwidth);
                                    //Instrument.SetWidth(newwidth);
                                    Jobid.SetWidth(newwidth);
                                    Noruns.SetWidth(newwidth);
                                }
                            }

                            function NavSplit()
                            {
                                OnClick('LPcell', 'separatorImage', true);
                                window.onbeforeunload = null;

                                window.addEventListener("beforeunload", (event) => {
                                    return null;
                                })
                            }

        function tbuirefresh() {
            OnClick('LPcell', 'separatorImage', true);
            if (!document.fullscreenElement) {
                document.documentElement.requestFullscreen();
                document.addEventListener('fullscreenchange', exitHandler);
                document.addEventListener('webkitfullscreenchange', exitHandler);
                document.addEventListener('mozfullscreenchange', exitHandler);
                document.addEventListener('MSFullscreenChange', exitHandler);
            }
            else {
                if (document.exitFullscreen) {
                    document.exitFullscreen();
                    document.removeEventListener('fullscreenchange', exitHandler);
                    document.removeEventListener('webkitfullscreenchange', exitHandler);
                    document.removeEventListener('mozfullscreenchange', exitHandler);
                    document.removeEventListener('MSFullscreenChange', exitHandler);
                }
            }
        }

        function plmuirefresh()
        {
            OnClick('LPcell', 'separatorImage', true);
            //var fbtn = document.getElementById('Vertical_TB_Menu_DXI3_');
            //if (fbtn != null) {
            //    if (fbtn.style.textAlign != "Center")
            //    {
            //        fbtn.style.textAlign = "Center";
            //        fbtn.style.verticalAlign = "middle";
            //    }
            //    console.log(fbtn);
            //    if (document.getElementById('LPcell').offsetWidth != 0) {
            //        fbtn.innerText = "Collapse";
            //        fbtn.title = "Collapse";
            //    } else {
            //        fbtn.innerText = "Expand";
            //        fbtn.title = "Expand";
            //    }
            //}
        }

        function plmtab()
        {
            var Tab = document.getElementsByClassName('LayoutTabContainer');
            if (Tab != null) {
                const Divs = Array.prototype.filter.call(
                    Tab,
                    (testElement) => testElement.nodeName === "DIV",
                );
                for (let i = 0; i < Divs.length; i++) {
                    Divs[i].style.height = screen.height * 2 / 3.725 + "px";
                }
            }
            //OnClick('LPcell', 'separatorImage', true);
            //var fbtn = document.getElementById('Vertical_TB_Menu_DXI3_');
            //if (fbtn != null && fbtn.title == "FullScreen") {
            //    plmuirefresh();
            //}
        }

        function exitHandler() {
            if (!document.fullscreenElement) {
                OnClick('LPcell', 'separatorImage', true);
                document.removeEventListener('fullscreenchange', exitHandler);
                document.removeEventListener('webkitfullscreenchange', exitHandler);
                document.removeEventListener('mozfullscreenchange', exitHandler);
                document.removeEventListener('MSFullscreenChange', exitHandler);
            }           
        }
                         function onKeyDown(e) {
                             isLeft = e.keyCode == 37;
                             isRight = e.keyCode == 39;
                             isUp = e.keyCode == 38;
                             isDown = e.keyCode == 40;
                             isEnter = e.keyCode == 13;

                             if (isLeft)
                                 Grid.batchEditApi.MoveFocusBackward();
                             if (isRight)
                                 Grid.batchEditApi.MoveFocusForward();
                             if (isDown)
                                 Grid.batchEditApi.StartEdit(rowIndex + 1, columnIndex);
                             if (isUp)
                                 Grid.batchEditApi.StartEdit(rowIndex - 1, columnIndex);
                             if (e.keyCode === 13)
                             {                                 
                                 e.stopPropagation();
                                 Grid.batchEditApi.StartEdit(rowIndex + 1, columnIndex);
                             }
                         }
                         function onStartEditing(s, e) {
                             columnIndex = e.focusedColumn.index;
                             rowIndex = e.visibleIndex;
                         }
                         function onInit(s, e) {
                             ASPxClientUtils.AttachEventToElement(Grid.GetMainElement(), "keydown", onKeyDown);
                         }

                         var FocusedCellColumnIndex = 0;
                         var FocusedCellRowIndex = 0;
                         function OnInit(s, e) {
                             ASPxClientUtils.AttachEventToElement(Grid.GetMainElement(), "keydown", function (evt) {
                                 if (evt.keyCode === ASPxClientUtils.StringToShortcutCode("UP"))
                                     UpPressed();
                                 else if (evt.keyCode === ASPxClientUtils.StringToShortcutCode("DOWN"))
                                     DownPressed();
                                 else if (evt.keyCode === ASPxClientUtils.StringToShortcutCode("RIGHT"))
                                     RightPressed();
                                 else if (evt.keyCode === ASPxClientUtils.StringToShortcutCode("LEFT"))
                                     LeftPressed();
                             });
                         }
                         function OnStartEditCell(s, e) {
                             FocusedCellColumnIndex = e.focusedColumn.index;
                             FocusedCellRowIndex = e.visibleIndex;
                         }
                         function OnEndEditCell(s, e) {
                             FocusedCellColumnIndex = 0;
                             FocusedCellRowIndex = 0;
                         }
                         function UpPressed() {
                             if (FocusedCellRowIndex > Grid.GetTopVisibleIndex())
                                 Grid.batchEditApi.StartEdit(FocusedCellRowIndex - 1, FocusedCellColumnIndex);
                             else
                                 Grid.batchEditApi.EndEdit();
                         }
                         function DownPressed() {
                             var lastRecordIndex = Grid.GetTopVisibleIndex() + Grid.GetVisibleRowsOnPage() - 1;
                             if (FocusedCellRowIndex < lastRecordIndex)
                                 Grid.batchEditApi.StartEdit(FocusedCellRowIndex + 1, FocusedCellColumnIndex);
                             else
                                 Grid.batchEditApi.EndEdit();
                         }
                         function RightPressed() {
                             if (FocusedCellColumnIndex < Grid.GetColumnCount())
                                 Grid.batchEditApi.StartEdit(FocusedCellRowIndex, FocusedCellColumnIndex + 1);
                             else
                                 Grid.batchEditApi.EndEdit();
                         }
                         function LeftPressed() {
                             if (FocusedCellColumnIndex > 0)
                                 Grid.batchEditApi.StartEdit(FocusedCellRowIndex, FocusedCellColumnIndex - 1);
                             else
                                 Grid.batchEditApi.EndEdit();
                         } 

        function onMethodNumberValueChanged(s, e) {
            var methodNumber = MethodNumber.GetValue();
            //var methodName = MethodNumber.GetText();
            //sessionStorage.setItem('MethodNumber', methodNumber);
            //sessionStorage.setItem('MethodName', methodName);
            var value = 'MethodChanged|' + methodNumber;
            RaiseXafCallback(globalCallbackControl, 'MethodChanged', value, '', false);
        }

        //function onSequenceLoaded(index) {
        //    if (sessionStorage.getItem('AllowSelectionByDataCell') == true) {
        //        RaiseXafCallback(globalCallbackControl, 'QC', 'SelectedRow|' + index, '', false)
        //    }
        //}

        function scroll() {
            if (GetActivePopupControl() != null && GetActivePopupControl().IsVisible()) {
                document.body.style.overflow = 'auto';
            }
            else {
                document.body.style.overflow = 'hidden';
                var oldDocumentRestoreBodyScroll = DocumentRestoreBodyScroll;
                DocumentRestoreBodyScroll = function () {
                    oldDocumentRestoreBodyScroll();
                    document.documentElement.removeAttribute("style");
                    document.documentElement.setAttribute("style", "overflow: hidden");
                }
            }
        }

    </script>
</head>
<body class="VerticalTemplate" onload="javascript:scroll()" onbeforeunload="DocumentRestoreBodyScroll">
    <form id="form2" runat="server">
        <cc4:aspxprogresscontrol id="ProgressControl" runat="server" />
        <div runat="server" id="Content" />
    </form>
</body>
</html>
