using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Permissions;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Model;

namespace SiteServer.BackgroundPages.Settings
{
    public class PagePermissionAdd : BasePageCms
    {
        public CheckBoxList CblWebsitePermissions;
        public CheckBoxList CblChannelPermissions;
        public Literal LtlNodeTree;

        public PlaceHolder PhWebsitePermissions;
        public PlaceHolder PhChannelPermissions;

        public static string GetRedirectUrl(int publishmentSystemId, string roleName)
        {
            var queryString = new NameValueCollection { { "PublishmentSystemID", publishmentSystemId.ToString() } };
            if (!string.IsNullOrEmpty(roleName))
            {
                queryString.Add("RoleName", roleName);
            }

            return PageUtils.GetSettingsUrl(nameof(PagePermissionAdd), queryString);
        }

        private string GetNodeTreeHtml()
        {
            var htmlBuilder = new StringBuilder();
            var systemPermissionsInfoList = Session[PageRoleAdd.SystemPermissionsInfoListKey] as List<SystemPermissionsInfo>;
            if (systemPermissionsInfoList == null)
            {
                PageUtils.RedirectToErrorPage("超出时间范围，请重新进入！");
                return string.Empty;
            }
            var nodeIdList = new List<int>();
            foreach (var systemPermissionsInfo in systemPermissionsInfoList)
            {
                nodeIdList.AddRange(TranslateUtils.StringCollectionToIntList(systemPermissionsInfo.NodeIdCollection));
            }

            var treeDirectoryUrl = SiteServerAssets.GetIconUrl("tree");

            htmlBuilder.Append("<span id='ChannelSelectControl'>");
            var theNodeIdList = DataProvider.NodeDao.GetNodeIdListByPublishmentSystemId(PublishmentSystemId);
            var isLastNodeArray = new bool[theNodeIdList.Count];
            foreach (var theNodeId in theNodeIdList)
            {
                var nodeInfo = NodeManager.GetNodeInfo(PublishmentSystemId, theNodeId);
                htmlBuilder.Append(GetTitle(nodeInfo, treeDirectoryUrl, isLastNodeArray, nodeIdList));
                htmlBuilder.Append("<br/>");
            }
            htmlBuilder.Append("</span>");
            return htmlBuilder.ToString();
        }

        private string GetTitle(NodeInfo nodeInfo, string treeDirectoryUrl, IList<bool> isLastNodeArray, ICollection<int> nodeIdList)
        {
            var itemBuilder = new StringBuilder();
            if (nodeInfo.NodeId == PublishmentSystemId)
            {
                nodeInfo.IsLastNode = true;
            }
            if (nodeInfo.IsLastNode == false)
            {
                isLastNodeArray[nodeInfo.ParentsCount] = false;
            }
            else
            {
                isLastNodeArray[nodeInfo.ParentsCount] = true;
            }
            for (var i = 0; i < nodeInfo.ParentsCount; i++)
            {
                itemBuilder.Append(isLastNodeArray[i]
                    ? $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_empty.gif\"/>"
                    : $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_line.gif\"/>");
            }
            if (nodeInfo.IsLastNode)
            {
                itemBuilder.Append(nodeInfo.ChildrenCount > 0
                    ? $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_plusbottom.gif\"/>"
                    : $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_minusbottom.gif\"/>");
            }
            else
            {
                itemBuilder.Append(nodeInfo.ChildrenCount > 0
                    ? $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_plusmiddle.gif\"/>"
                    : $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_minusmiddle.gif\"/>");
            }

            var check = "";
            if (nodeIdList.Contains(nodeInfo.NodeId))
            {
                check = "checked";
            }

            var disabled = "";
            if (!IsOwningNodeId(nodeInfo.NodeId))
            {
                disabled = "disabled";
                check = "";
            }

            itemBuilder.Append(
                $@"<label class=""checkbox inline""><input type=""checkbox"" name=""NodeIDCollection"" value=""{nodeInfo
                    .NodeId}"" {check} {disabled}/> {nodeInfo.NodeName} &nbsp;<span style=""font-size:8pt;font-family:arial"" class=""gray"">({nodeInfo.ContentNum})</span></label>");

            return itemBuilder.ToString();
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            var permissioins = PermissionsManager.GetPermissions(Body.AdminName);
            LtlNodeTree.Text = GetNodeTreeHtml();

            if (IsPostBack) return;

            PermissionsManager.VerifyAdministratorPermissions(Body.AdminName, AppManager.Permissions.Settings.AdminManagement);

            if (permissioins.IsSystemAdministrator)
            {
                var channelPermissions = PermissionConfigManager.Instance.ChannelPermissions;
                foreach (var permission in channelPermissions)
                {
                    if (permission.Name == AppManager.Permissions.Channel.ContentCheckLevel1)
                    {
                        if (PublishmentSystemInfo.IsCheckContentUseLevel)
                        {
                            if (PublishmentSystemInfo.CheckContentLevel < 1)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (permission.Name == AppManager.Permissions.Channel.ContentCheckLevel2)
                    {
                        if (PublishmentSystemInfo.IsCheckContentUseLevel)
                        {
                            if (PublishmentSystemInfo.CheckContentLevel < 2)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (permission.Name == AppManager.Permissions.Channel.ContentCheckLevel3)
                    {
                        if (PublishmentSystemInfo.IsCheckContentUseLevel)
                        {
                            if (PublishmentSystemInfo.CheckContentLevel < 3)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (permission.Name == AppManager.Permissions.Channel.ContentCheckLevel4)
                    {
                        if (PublishmentSystemInfo.IsCheckContentUseLevel)
                        {
                            if (PublishmentSystemInfo.CheckContentLevel < 4)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (permission.Name == AppManager.Permissions.Channel.ContentCheckLevel5)
                    {
                        if (PublishmentSystemInfo.IsCheckContentUseLevel)
                        {
                            if (PublishmentSystemInfo.CheckContentLevel < 5)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    var listItem = new ListItem(permission.Text, permission.Name);
                    CblChannelPermissions.Items.Add(listItem);
                }
            }
            else
            {
                PhChannelPermissions.Visible = false;
                if (ProductPermissionsManager.Current.ChannelPermissionDict.ContainsKey(PublishmentSystemId))
                {
                    var channelPermissions = ProductPermissionsManager.Current.ChannelPermissionDict[PublishmentSystemId];
                    foreach (var channelPermission in channelPermissions)
                    {
                        foreach (var permission in PermissionConfigManager.Instance.ChannelPermissions)
                        {
                            if (permission.Name == channelPermission)
                            {
                                if (channelPermission == AppManager.Permissions.Channel.ContentCheck)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel) continue;
                                }
                                else if (channelPermission == AppManager.Permissions.Channel.ContentCheckLevel1)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel == false || PublishmentSystemInfo.CheckContentLevel < 1) continue;
                                }
                                else if (channelPermission == AppManager.Permissions.Channel.ContentCheckLevel2)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel == false || PublishmentSystemInfo.CheckContentLevel < 2) continue;
                                }
                                else if (channelPermission == AppManager.Permissions.Channel.ContentCheckLevel3)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel == false || PublishmentSystemInfo.CheckContentLevel < 3) continue;
                                }
                                else if (channelPermission == AppManager.Permissions.Channel.ContentCheckLevel4)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel == false || PublishmentSystemInfo.CheckContentLevel < 4) continue;
                                }
                                else if (channelPermission == AppManager.Permissions.Channel.ContentCheckLevel5)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel == false || PublishmentSystemInfo.CheckContentLevel < 5) continue;
                                }

                                PhChannelPermissions.Visible = true;
                                var listItem = new ListItem(permission.Text, permission.Name);
                                CblChannelPermissions.Items.Add(listItem);
                            }
                        }
                    }
                }
            }

            if (permissioins.IsSystemAdministrator)
            {
                var websitePermissions = PermissionConfigManager.Instance.WebsitePermissions;
                foreach (var permission in websitePermissions)
                {
                    var listItem = new ListItem(permission.Text, permission.Name);
                    CblWebsitePermissions.Items.Add(listItem);
                }
            }
            else
            {
                PhWebsitePermissions.Visible = false;
                if (ProductPermissionsManager.Current.WebsitePermissionDict.ContainsKey(PublishmentSystemId))
                {
                    var websitePermissionList = ProductPermissionsManager.Current.WebsitePermissionDict[PublishmentSystemId];
                    foreach (var websitePermission in websitePermissionList)
                    {
                        foreach (var permission in PermissionConfigManager.Instance.WebsitePermissions)
                        {
                            if (permission.Name == websitePermission)
                            {
                                PhWebsitePermissions.Visible = true;
                                var listItem = new ListItem(permission.Text, permission.Name);
                                CblWebsitePermissions.Items.Add(listItem);
                            }
                        }
                    }
                }
            }

            var systemPermissionsInfoList = Session[PageRoleAdd.SystemPermissionsInfoListKey] as List<SystemPermissionsInfo>;
            if (systemPermissionsInfoList != null)
            {
                SystemPermissionsInfo systemPermissionsInfo = null;
                foreach (var publishmentSystemPermissionsInfo in systemPermissionsInfoList)
                {
                    if (publishmentSystemPermissionsInfo.PublishmentSystemId == PublishmentSystemId)
                    {
                        systemPermissionsInfo = publishmentSystemPermissionsInfo;
                        break;
                    }
                }
                if (systemPermissionsInfo == null) return;

                foreach (ListItem item in CblChannelPermissions.Items)
                {
                    item.Selected = CompareUtils.Contains(systemPermissionsInfo.ChannelPermissions, item.Value);
                }
                foreach (ListItem item in CblWebsitePermissions.Items)
                {
                    item.Selected = CompareUtils.Contains(systemPermissionsInfo.WebsitePermissions, item.Value);
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (!Page.IsPostBack || !Page.IsValid) return;

            var systemPermissionsInfoList = Session[PageRoleAdd.SystemPermissionsInfoListKey] as List<SystemPermissionsInfo>;
            if (systemPermissionsInfoList != null)
            {
                var systemPermissionlist = new List<SystemPermissionsInfo>();
                foreach (var systemPermissionsInfo in systemPermissionsInfoList)
                {
                    if (systemPermissionsInfo.PublishmentSystemId == PublishmentSystemId)
                    {
                        continue;
                    }
                    systemPermissionlist.Add(systemPermissionsInfo);
                }

                var nodeIdList = TranslateUtils.StringCollectionToStringList(Request.Form["NodeIDCollection"]);
                if (nodeIdList.Count > 0 && CblChannelPermissions.SelectedItem != null || CblWebsitePermissions.SelectedItem != null)
                {
                    var systemPermissionsInfo = new SystemPermissionsInfo
                    {
                        PublishmentSystemId = PublishmentSystemId,
                        NodeIdCollection = TranslateUtils.ObjectCollectionToString(nodeIdList),
                        ChannelPermissions =
                            ControlUtils.SelectedItemsValueToStringCollection(CblChannelPermissions.Items),
                        WebsitePermissions =
                            ControlUtils.SelectedItemsValueToStringCollection(CblWebsitePermissions.Items)
                    };

                    systemPermissionlist.Add(systemPermissionsInfo);
                }

                Session[PageRoleAdd.SystemPermissionsInfoListKey] = systemPermissionlist;
            }

            PageUtils.Redirect(PageRoleAdd.GetReturnRedirectUrl(Body.GetQueryString("RoleName")));
        }

        public void Return_OnClick(object sender, EventArgs e)
        {
            PageUtils.Redirect(PageRoleAdd.GetReturnRedirectUrl(Body.GetQueryString("RoleName")));
        }
    }
}
