
 
 

	 


namespace WeiCode.ModelDbs
{
    using System;
    using System.Collections.Generic;
	using WeiCode.Utility;
    using WeiCode.DataBase;

    //内置全局枚举
public partial class ModelEnum {
		/// <summary>
     ///用户类型
		/// </summary>
	public enum UserTypeEnum {
		/// <summary>
		///超管
		/// </summary>
		manager=9,
		/// <summary>
		///厅管
		/// </summary>
		tger=10,
		/// <summary>
		///主播
		/// </summary>
		zber=11,
		/// <summary>
		///运营
		/// </summary>
		yyer=12,
		/// <summary>
		///流量
		/// </summary>
		wxer=13,
		/// <summary>
		///签约
		/// </summary>
		qyer=14,
		/// <summary>
		///萌新
		/// </summary>
		mxer=15,
		/// <summary>
		///API
		/// </summary>
		api=16,
		/// <summary>
		///基地
		/// </summary>
		jder=17,
		/// <summary>
		///中台
		/// </summary>
		zter=18,
	}
		/// <summary>
     ///字典类别
		/// </summary>
	public enum DictCategory {
		/// <summary>
		///用户等级
		/// </summary>
		用户等级=8,
		/// <summary>
		///与用户的认识方式
		/// </summary>
		与用户的认识方式=9,
		/// <summary>
		///用户生活作息
		/// </summary>
		用户生活作息=10,
		/// <summary>
		///场次
		/// </summary>
		场次=11,
		/// <summary>
		///星座
		/// </summary>
		星座=12,
		/// <summary>
		///MBTI
		/// </summary>
		MBTI=13,
		/// <summary>
		///场次类型
		/// </summary>
		场次类型=14,
		/// <summary>
		///场次类型运营
		/// </summary>
		场次类型运营=15,
		/// <summary>
		///档位角色
		/// </summary>
		档位角色=16,
		/// <summary>
		///档位时段
		/// </summary>
		档位时段=19,
		/// <summary>
		///职务
		/// </summary>
		职务=20,
		/// <summary>
		///学历
		/// </summary>
		学历=21,
		/// <summary>
		///招聘渠道
		/// </summary>
		招聘渠道=22,
		/// <summary>
		///直播间类型
		/// </summary>
		直播间类型=23,
	}
		/// <summary>
     ///用户关系类型枚举
		/// </summary>
	public enum UserRelationTypeEnum {
		/// <summary>
		///同一运营团队下面的A厅主播转移到B厅
		/// </summary>
		厅管邀主播=1,
		/// <summary>
		///不同运营团队厅转移（若该厅有分裂，需单独说明报备）
		/// </summary>
		运营邀厅管=2,
		/// <summary>
		///同一运营团队下面该厅从多厅A转移至多厅B
		/// </summary>
		厅管邀厅管=3,
		/// <summary>
		///
		/// </summary>
		基地邀运营=4,
	}
}
}
 
