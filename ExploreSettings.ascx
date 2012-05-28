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

        self.htmlEncode = function(value) {
            return $('<div/>').text(value).html();
        };

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
                    s += "<li>" + self.htmlEncode(key) + ":" + self.htmlEncode(self.data[key]) + "</li>";
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

        self.updateKey = function() {
            var postData = { key: $("#key").text(), value: $("#value").val() };
            $.ajax({
                type: "POST",
                url: sf.getServiceRoot('ExploreSettings') + "Settings.ashx/UpdateHostSetting",
                data: sf.getAntiForgeryProperty(postData),
                beforeSend: sf.setModuleHeaders,
                error: function(xhr, status, error) {
                    alert(error);
                }
            }).done(function() {
                self.loadSettings();
            }).fail(function () {
                alert("Uh-oh, something broke");
            });
        };

        self.loadSettings();
    })
</script>