<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExploreSettings.ascx.cs" Inherits="ExploreSettings.ExploreSettings" %>
<asp:Panel runat="server" ID="ScopeWrapper">
    <div>
        <select id="settingSelect">
            <option value="HostSettings">
                Host
            </option>
            <option value="CurrentPortalSettings">
                Portal
            </option>
        </select>
        <ul id="settings" />
    </div>
</asp:Panel>
<script type="text/javascript">
    $(document).ready(function () {
        var self = this;
        var moduleScope = $('#<%=ScopeWrapper.ClientID %>');
        var sf = $.ServicesFramework(<%=ModuleId %>);
       
        $("#settingSelect", moduleScope).change(function() {
            self.LoadSettings();
        });

        self.htmlEncode = function(value) {
            return $('<div/>').text(value).html();
        };

        self.LoadSettings = function() {
            var action = $("#settingSelect").val();
            
            $.ajax({
                type: "GET",
                url: sf.getServiceRoot('ExploreSettings') + "Settings.ashx/" + action,
                data: '',
                beforeSend: sf.setModuleHeaders,
                error: function(xhr, status, error) {
                    alert(error);
                }
            }).done(function(data) {
                if (data !== undefined && data != null) {
                    var s = "";
                    for (key in data) {
                        s += "<li>" + self.htmlEncode(key) + ":" + self.htmlEncode(data[key]) + "</li>";
                    }
                    $("#settings", moduleScope).empty().append(s);
                }
            });
        };

        self.LoadSettings();
    })
</script>