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
        Filter: <input type="text" id="filter"/>
        <ul id="settings" />
    </div>
</asp:Panel>
<script type="text/javascript">
    $(document).ready(function () {
        var self = this;
        var moduleScope = $('#<%=ScopeWrapper.ClientID %>');
        var sf = $.ServicesFramework(<%=ModuleId %>);
       
        $("#settingSelect", moduleScope).change(function() {
            self.loadSettings();
        });

        $("#filter").keyup(function() {
            self.displayData();
        });

        self.htmlEncode = function(value) {
            return $('<div/>').text(value).html();
        };

        self.displayData = function() {
            if(!self.data) {
                return;
            }

            var filter = $("#filter").val().toLowerCase();
            var s = "";
            for (key in self.data) {
                var include = true;
                if(filter) {
                    include = key.toLowerCase().indexOf(filter) > -1 || self.data[key].toLowerCase().indexOf(filter) > -1;
                }
                        
                if(include) {
                    s += "<li>" + self.htmlEncode(key) + ":" + self.htmlEncode(self.data[key]) + "</li>";
                }
            }
            
            if(!s) {
                s = "<li>No Settings!!!</li>";
            }

            $("#settings", moduleScope).empty().append(s);
        };

        self.loadSettings = function() {
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
                
                    self.data = data;
                    self.displayData();
                }
            });
        };

        self.loadSettings();
    })
</script>