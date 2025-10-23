using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Modular;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        #region UserBaseTList
        /// <summary>
        /// 账号列表扩展
        /// </summary>
        public class UserBaseTList : UserBaseTpl
        {
            #region DefaultView
            /// <summary>
            /// 设置列表筛选表单的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            protected override CtlListFilter GetListFilter(UserBaseTplPara req = null)
            {
                req.listFilter.formItems[2].disabled = true;//禁用时间筛选
                return req.listFilter;
            }

            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            protected override EmtButtonGroup GetButtonGroup(UserBaseTplPara req = null)
            {
                return req.buttonGroup;
            }

            /// <summary>
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            protected override CtlListDisplay GetListDisplay(UserBaseTplPara req = null)
            {
                //1.判断当前用户类型，如果是厅管则显示上级部门,主播，运营则不显示
                if((ModelEnum.UserTypeEnum)req.user_type_id == ModelEnum.UserTypeEnum.tger)
                {
                    //2.获取厅管的上级运营人员的名称
                    req.listDisplay.listItems.Add(new EmtModel.ListItem("SuperiorDepartment")
                    {
                        text = "上级部门",
                        width = "200",
                        minWidth = "200",
                        index = 3
                    });
                }
                req.listDisplay.listData = new CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                #region 操作列按钮

                #endregion
                return req.listDisplay;
            }
            #endregion

            #region AJAX函数:ListData
            /// <summary>
            /// data数据
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
            {
                var req = new PageList.ListData.Req();
                req.pager = reqJson.pager;
                return new CtlListDisplay.ListData().getList<ModelDbModularBasic.user_base, ItemDataModel>(new DoMySql.Filter
                {
                    where = "1=1"
                }, reqJson);
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : UserBaseTpl.ItemDataModelBase
            {
                public string SuperiorDepartment
                {
                    get
                    {
                        return null;
                        //todo: new DomainBasic.UserApp().GetInfoByUserSn(new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, this.user_sn)).username
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
