<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExploreSettings.ascx.cs" Inherits="ExploreSettings.ExploreSettings" %>
<asp:Panel runat="server" ID="ScopeWrapper">
    <div>
        <select id="settingSelect">
            <% if (UserInfo.IsSuperUser) { %>
                <option value="0">
                    Host
                </option>
            <%  } %>
            <option value="1">
                Portal
            </option>
        </select>
        Filter: <input type="text" id="filter"/>
        <span id="editBlock">
            <span id="key"></span>
            <input type="text" id="value"/>
            <input type="submit" id="updateKey" value="Update Key"/>
            <input type="submit" id="cancelEdit" value="Cancel"/>
        </span>
        <ul id="settings" />
    </div>
</asp:Panel>
<script type="text/javascript">
    $(document).ready(function () {
        var self = this;
        var moduleScope = $('#<%=ScopeWrapper.ClientID %>');
        var sf = $.ServicesFramework(<%=ModuleId %>);
        $("#editBlock").hide();
       
        $("#settingSelect", moduleScope).change(function() {
            $("#filter").val("");
            self.loadSettings();
        });

        $("#filter").keyup(function() {
            self.displayData();
        });

        $("#updateKey").click(function() {
            self.updateKey();
            $("#editBlock").hide();
            return false;
        });

        $("#cancelEdit").click(function() {
            $("#editBlock").hide();
            return false;
        });

        self.editItem = function(elem) {
            var items = elem.textContent.split(':');
            $("#key").text(items[0]);
            $("#value").val(items[1]);
            $("#editBlock").show();
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
                    s += "<li>" +key + ":" + self.data[key] + "</li>";
                }
            }
            
            if(!s) {
                s = "<li>No Settings!!!</li>";
            }

            $("#settings", moduleScope).empty().append(s);
            $("li", moduleScope).click(function() {
                self.editItem(this);
            });
        };

        self.getLoadAction = function() {
            if($("#settingSelect").val() === "1") {
                return "CurrentPortalSettings";    
            }
            return "HostSettings";
        };

        self.loadSettings = function() {
            var action = self.getLoadAction();
            
            $.ajax({
                type: "GET",
                url: sf.getServiceRoot('ExploreSettings') + "Settings.ashx/" + action,
                beforeSend: sf.setModuleHeaders
            }).done(function(data) {
                if (data !== undefined && data != null) {
                
                    self.data = data;
                    self.displayData();
                }
            }).fail(function (xhr, result, status) {
                alert("Uh-oh, something broke: " + status);
            });
        };

        self.getUpdateAction = function() {
            if($("#settingSelect").val() === "1") {
                return "UpdatePortalSetting";
            }
            return "UpdateHostSetting";
        };

        self.updateKey = function() {
            var postData = { key: $("#key").text(), value: $("#value").val() };
            var action = self.getUpdateAction();
            
            $.ajax({
                type: "POST",
                url: sf.getServiceRoot('ExploreSettings') + "Settings.ashx/" + action,
                data: sf.getAntiForgeryProperty(postData),
                beforeSend: sf.setModuleHeaders
            }).done(function() {
                self.loadSettings();
            }).fail(function (xhr, result, status) {
                alert("Uh-oh, something broke: " + status);
            });
        };

        self.loadSettings();
    })
</script>