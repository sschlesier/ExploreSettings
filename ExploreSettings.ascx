<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExploreSettings.ascx.cs" Inherits="ExploreSettings.ExploreSettings" %>
<asp:Panel runat="server" ID="ScopeWrapper">
    <div>
        Host Settings
        <ul id="hostSettings" />
    </div>
    <div>
        Portal Settings
        <ul id="portalSettings" />
    </div>
</asp:Panel>
<script type="text/javascript">
    $(document).ready(function () {
        var moduleScope = $('#<%=ScopeWrapper.ClientID %>');
        var sf = $.ServicesFramework(<%=ModuleId %>);

        $.ajax({
            type: "GET",
            url: sf.getServiceRoot('ExploreSettings') + "Settings.ashx/HostSettings",
            data: '',
            beforeSend: sf.setModuleHeaders,
            complete: function (xhr) {
                if (xhr.status == 200) {
                    $("#hostSettings", moduleScope).text(xhr.responseText);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });

    })
</script>