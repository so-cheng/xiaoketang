
 
 

	 


namespace WeiCode.ModelDbs
{
    using System;
    using System.Collections.Generic;
	using WeiCode.Utility;
    using WeiCode.DataBase;
	using WeiCode.Services;
	using WeiCode.Domain;

	public class ModelDb {

    			/// <summary>
			/// 表实体-绩效-目标 
			/// </summary>	
			public class _p_jixiao_target : ModelDbBase
			{    
							public _p_jixiao_target(){}
				public _p_jixiao_target(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.amount =  1m;
														this.new_num =  1;
														this.amount_2 =  1m;
														this.num_2 =  1;
														this.contact_num =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 目标月份
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 目标音浪
		/// </summary>
		public Nullable<decimal> amount{ get; set; }
		/// <summary>
		/// 目标拉新
		/// </summary>
		public Nullable<int> new_num{ get; set; }
		/// <summary>
		/// 目标二消
		/// </summary>
		public Nullable<decimal> amount_2{ get; set; }
		/// <summary>
		/// 目标二消数
		/// </summary>
		public Nullable<int> num_2{ get; set; }
		/// <summary>
		/// 目标建联数
		/// </summary>
		public Nullable<int> contact_num{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-日目标-建联 
			/// </summary>	
			public class _p_jixiao_target_contact : ModelDbBase
			{    
							public _p_jixiao_target_contact(){}
				public _p_jixiao_target_contact(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.contact_1 =  1m;
														this.contact_2 =  1m;
														this.contact_3 =  1m;
														this.contact_4 =  1m;
														this.contact_5 =  1m;
														this.contact_6 =  1m;
														this.contact_7 =  1m;
														this.contact_8 =  1m;
														this.contact_9 =  1m;
														this.contact_10 =  1m;
														this.contact_11 =  1m;
														this.contact_12 =  1m;
														this.contact_13 =  1m;
														this.contact_14 =  1m;
														this.contact_15 =  1m;
														this.contact_16 =  1m;
														this.contact_17 =  1m;
														this.contact_18 =  1m;
														this.contact_19 =  1m;
														this.contact_20 =  1m;
														this.contact_21 =  1m;
														this.contact_22 =  1m;
														this.contact_23 =  1m;
														this.contact_24 =  1m;
														this.contact_25 =  1m;
														this.contact_26 =  1m;
														this.contact_27 =  1m;
														this.contact_28 =  1m;
														this.contact_29 =  1m;
														this.contact_30 =  1m;
														this.contact_31 =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 目标月份
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 目标音浪1
		/// </summary>
		public Nullable<decimal> contact_1{ get; set; }
		/// <summary>
		/// 目标音浪2
		/// </summary>
		public Nullable<decimal> contact_2{ get; set; }
		/// <summary>
		/// 目标音浪3
		/// </summary>
		public Nullable<decimal> contact_3{ get; set; }
		/// <summary>
		/// 目标音浪4
		/// </summary>
		public Nullable<decimal> contact_4{ get; set; }
		/// <summary>
		/// 目标音浪5
		/// </summary>
		public Nullable<decimal> contact_5{ get; set; }
		/// <summary>
		/// 目标音浪6
		/// </summary>
		public Nullable<decimal> contact_6{ get; set; }
		/// <summary>
		/// 目标音浪7
		/// </summary>
		public Nullable<decimal> contact_7{ get; set; }
		/// <summary>
		/// 目标音浪8
		/// </summary>
		public Nullable<decimal> contact_8{ get; set; }
		/// <summary>
		/// 目标音浪9
		/// </summary>
		public Nullable<decimal> contact_9{ get; set; }
		/// <summary>
		/// 目标音浪10
		/// </summary>
		public Nullable<decimal> contact_10{ get; set; }
		/// <summary>
		/// 目标音浪11
		/// </summary>
		public Nullable<decimal> contact_11{ get; set; }
		/// <summary>
		/// 目标音浪12
		/// </summary>
		public Nullable<decimal> contact_12{ get; set; }
		/// <summary>
		/// 目标音浪13
		/// </summary>
		public Nullable<decimal> contact_13{ get; set; }
		/// <summary>
		/// 目标音浪14
		/// </summary>
		public Nullable<decimal> contact_14{ get; set; }
		/// <summary>
		/// 目标音浪15
		/// </summary>
		public Nullable<decimal> contact_15{ get; set; }
		/// <summary>
		/// 目标音浪16
		/// </summary>
		public Nullable<decimal> contact_16{ get; set; }
		/// <summary>
		/// 目标音浪17
		/// </summary>
		public Nullable<decimal> contact_17{ get; set; }
		/// <summary>
		/// 目标音浪18
		/// </summary>
		public Nullable<decimal> contact_18{ get; set; }
		/// <summary>
		/// 目标音浪19
		/// </summary>
		public Nullable<decimal> contact_19{ get; set; }
		/// <summary>
		/// 目标音浪20
		/// </summary>
		public Nullable<decimal> contact_20{ get; set; }
		/// <summary>
		/// 目标音浪21
		/// </summary>
		public Nullable<decimal> contact_21{ get; set; }
		/// <summary>
		/// 目标音浪22
		/// </summary>
		public Nullable<decimal> contact_22{ get; set; }
		/// <summary>
		/// 目标音浪23
		/// </summary>
		public Nullable<decimal> contact_23{ get; set; }
		/// <summary>
		/// 目标音浪24
		/// </summary>
		public Nullable<decimal> contact_24{ get; set; }
		/// <summary>
		/// 目标音浪25
		/// </summary>
		public Nullable<decimal> contact_25{ get; set; }
		/// <summary>
		/// 目标音浪26
		/// </summary>
		public Nullable<decimal> contact_26{ get; set; }
		/// <summary>
		/// 目标音浪27
		/// </summary>
		public Nullable<decimal> contact_27{ get; set; }
		/// <summary>
		/// 目标音浪28
		/// </summary>
		public Nullable<decimal> contact_28{ get; set; }
		/// <summary>
		/// 目标音浪29
		/// </summary>
		public Nullable<decimal> contact_29{ get; set; }
		/// <summary>
		/// 目标音浪30
		/// </summary>
		public Nullable<decimal> contact_30{ get; set; }
		/// <summary>
		/// 目标音浪31
		/// </summary>
		public Nullable<decimal> contact_31{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-日目标-首消 
			/// </summary>	
			public class _p_jixiao_target_day : ModelDbBase
			{    
							public _p_jixiao_target_day(){}
				public _p_jixiao_target_day(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.amount_1 =  1m;
														this.amount_2 =  1m;
														this.amount_3 =  1m;
														this.amount_4 =  1m;
														this.amount_5 =  1m;
														this.amount_6 =  1m;
														this.amount_7 =  1m;
														this.amount_8 =  1m;
														this.amount_9 =  1m;
														this.amount_10 =  1m;
														this.amount_11 =  1m;
														this.amount_12 =  1m;
														this.amount_13 =  1m;
														this.amount_14 =  1m;
														this.amount_15 =  1m;
														this.amount_16 =  1m;
														this.amount_17 =  1m;
														this.amount_18 =  1m;
														this.amount_19 =  1m;
														this.amount_20 =  1m;
														this.amount_21 =  1m;
														this.amount_22 =  1m;
														this.amount_23 =  1m;
														this.amount_24 =  1m;
														this.amount_25 =  1m;
														this.amount_26 =  1m;
														this.amount_27 =  1m;
														this.amount_28 =  1m;
														this.amount_29 =  1m;
														this.amount_30 =  1m;
														this.amount_31 =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 目标月份
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 目标音浪1
		/// </summary>
		public Nullable<decimal> amount_1{ get; set; }
		/// <summary>
		/// 目标音浪2
		/// </summary>
		public Nullable<decimal> amount_2{ get; set; }
		/// <summary>
		/// 目标音浪3
		/// </summary>
		public Nullable<decimal> amount_3{ get; set; }
		/// <summary>
		/// 目标音浪4
		/// </summary>
		public Nullable<decimal> amount_4{ get; set; }
		/// <summary>
		/// 目标音浪5
		/// </summary>
		public Nullable<decimal> amount_5{ get; set; }
		/// <summary>
		/// 目标音浪6
		/// </summary>
		public Nullable<decimal> amount_6{ get; set; }
		/// <summary>
		/// 目标音浪7
		/// </summary>
		public Nullable<decimal> amount_7{ get; set; }
		/// <summary>
		/// 目标音浪8
		/// </summary>
		public Nullable<decimal> amount_8{ get; set; }
		/// <summary>
		/// 目标音浪9
		/// </summary>
		public Nullable<decimal> amount_9{ get; set; }
		/// <summary>
		/// 目标音浪10
		/// </summary>
		public Nullable<decimal> amount_10{ get; set; }
		/// <summary>
		/// 目标音浪11
		/// </summary>
		public Nullable<decimal> amount_11{ get; set; }
		/// <summary>
		/// 目标音浪12
		/// </summary>
		public Nullable<decimal> amount_12{ get; set; }
		/// <summary>
		/// 目标音浪13
		/// </summary>
		public Nullable<decimal> amount_13{ get; set; }
		/// <summary>
		/// 目标音浪14
		/// </summary>
		public Nullable<decimal> amount_14{ get; set; }
		/// <summary>
		/// 目标音浪15
		/// </summary>
		public Nullable<decimal> amount_15{ get; set; }
		/// <summary>
		/// 目标音浪16
		/// </summary>
		public Nullable<decimal> amount_16{ get; set; }
		/// <summary>
		/// 目标音浪17
		/// </summary>
		public Nullable<decimal> amount_17{ get; set; }
		/// <summary>
		/// 目标音浪18
		/// </summary>
		public Nullable<decimal> amount_18{ get; set; }
		/// <summary>
		/// 目标音浪19
		/// </summary>
		public Nullable<decimal> amount_19{ get; set; }
		/// <summary>
		/// 目标音浪20
		/// </summary>
		public Nullable<decimal> amount_20{ get; set; }
		/// <summary>
		/// 目标音浪21
		/// </summary>
		public Nullable<decimal> amount_21{ get; set; }
		/// <summary>
		/// 目标音浪22
		/// </summary>
		public Nullable<decimal> amount_22{ get; set; }
		/// <summary>
		/// 目标音浪23
		/// </summary>
		public Nullable<decimal> amount_23{ get; set; }
		/// <summary>
		/// 目标音浪24
		/// </summary>
		public Nullable<decimal> amount_24{ get; set; }
		/// <summary>
		/// 目标音浪25
		/// </summary>
		public Nullable<decimal> amount_25{ get; set; }
		/// <summary>
		/// 目标音浪26
		/// </summary>
		public Nullable<decimal> amount_26{ get; set; }
		/// <summary>
		/// 目标音浪27
		/// </summary>
		public Nullable<decimal> amount_27{ get; set; }
		/// <summary>
		/// 目标音浪28
		/// </summary>
		public Nullable<decimal> amount_28{ get; set; }
		/// <summary>
		/// 目标音浪29
		/// </summary>
		public Nullable<decimal> amount_29{ get; set; }
		/// <summary>
		/// 目标音浪30
		/// </summary>
		public Nullable<decimal> amount_30{ get; set; }
		/// <summary>
		/// 目标音浪31
		/// </summary>
		public Nullable<decimal> amount_31{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-月度-厅-完成情况(从doudata_round_ting中通过定时任务生成)doudata_day_ting_31 
			/// </summary>	
			public class _p_jixiao_target_month_ting : ModelDbBase
			{    
							public _p_jixiao_target_month_ting(){}
				public _p_jixiao_target_month_ting(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.tenday_income_1 =  1m;
														this.tenday_income_avg_1 =  1m;
														this.tenday_income_proportion_1 =  1m;
														this.undone_income_1 =  1m;
														this.tenday_income_2 =  1m;
														this.tenday_income_avg_2 =  1m;
														this.tenday_income_proportion_2 =  1m;
														this.undone_income_2 =  1m;
														this.tenday_income_3 =  1m;
														this.tenday_income_avg_3 =  1m;
														this.tenday_income_proportion_3 =  1m;
														this.undone_income_3 =  1m;
														this.total_income =  1m;
														this.total_income_avg =  1m;
														this.tenday_income_proportion =  1m;
														this.undone_income =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属中台sn
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 运营名
		/// </summary>
		public string yy_name{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 厅管名
		/// </summary>
		public string tg_name{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 厅名
		/// </summary>
		public string ting_name{ get; set; }
		/// <summary>
		/// 抖音号
		/// </summary>
		public string dou_user{ get; set; }
		/// <summary>
		/// 音浪发生年月
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 十日完成音浪1
		/// </summary>
		public Nullable<decimal> tenday_income_1{ get; set; }
		/// <summary>
		/// 十日日均音浪1=十日完成音浪1/10
		/// </summary>
		public Nullable<decimal> tenday_income_avg_1{ get; set; }
		/// <summary>
		/// 第一阶段完成占比
		/// </summary>
		public Nullable<decimal> tenday_income_proportion_1{ get; set; }
		/// <summary>
		/// 第一阶段未完成音浪
		/// </summary>
		public Nullable<decimal> undone_income_1{ get; set; }
		/// <summary>
		/// 十日完成音浪2
		/// </summary>
		public Nullable<decimal> tenday_income_2{ get; set; }
		/// <summary>
		/// 十日日均音浪2
		/// </summary>
		public Nullable<decimal> tenday_income_avg_2{ get; set; }
		/// <summary>
		/// 第二阶段完成占比
		/// </summary>
		public Nullable<decimal> tenday_income_proportion_2{ get; set; }
		/// <summary>
		/// 第二阶段未完成音浪
		/// </summary>
		public Nullable<decimal> undone_income_2{ get; set; }
		/// <summary>
		/// 十日音浪3
		/// </summary>
		public Nullable<decimal> tenday_income_3{ get; set; }
		/// <summary>
		/// 十日日均音浪3
		/// </summary>
		public Nullable<decimal> tenday_income_avg_3{ get; set; }
		/// <summary>
		/// 第三阶段完成占比
		/// </summary>
		public Nullable<decimal> tenday_income_proportion_3{ get; set; }
		/// <summary>
		/// 第三阶段未完成音浪
		/// </summary>
		public Nullable<decimal> undone_income_3{ get; set; }
		/// <summary>
		/// 总音浪
		/// </summary>
		public Nullable<decimal> total_income{ get; set; }
		/// <summary>
		/// 日均音浪
		/// </summary>
		public Nullable<decimal> total_income_avg{ get; set; }
		/// <summary>
		/// 月度完成占比
		/// </summary>
		public Nullable<decimal> tenday_income_proportion{ get; set; }
		/// <summary>
		/// 月度未完成音浪
		/// </summary>
		public Nullable<decimal> undone_income{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-日目标-拉新 
			/// </summary>	
			public class _p_jixiao_target_new : ModelDbBase
			{    
							public _p_jixiao_target_new(){}
				public _p_jixiao_target_new(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.new_1 =  1m;
														this.new_2 =  1m;
														this.new_3 =  1m;
														this.new_4 =  1m;
														this.new_5 =  1m;
														this.new_6 =  1m;
														this.new_7 =  1m;
														this.new_8 =  1m;
														this.new_9 =  1m;
														this.new_10 =  1m;
														this.new_11 =  1m;
														this.new_12 =  1m;
														this.new_13 =  1m;
														this.new_14 =  1m;
														this.new_15 =  1m;
														this.new_16 =  1m;
														this.new_17 =  1m;
														this.new_18 =  1m;
														this.new_19 =  1m;
														this.new_20 =  1m;
														this.new_21 =  1m;
														this.new_22 =  1m;
														this.new_23 =  1m;
														this.new_24 =  1m;
														this.new_25 =  1m;
														this.new_26 =  1m;
														this.new_27 =  1m;
														this.new_28 =  1m;
														this.new_29 =  1m;
														this.new_30 =  1m;
														this.new_31 =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 目标月份
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 目标拉新1
		/// </summary>
		public Nullable<decimal> new_1{ get; set; }
		/// <summary>
		/// 目标拉新2
		/// </summary>
		public Nullable<decimal> new_2{ get; set; }
		/// <summary>
		/// 目标拉新3
		/// </summary>
		public Nullable<decimal> new_3{ get; set; }
		/// <summary>
		/// 目标拉新4
		/// </summary>
		public Nullable<decimal> new_4{ get; set; }
		/// <summary>
		/// 目标拉新5
		/// </summary>
		public Nullable<decimal> new_5{ get; set; }
		/// <summary>
		/// 目标拉新6
		/// </summary>
		public Nullable<decimal> new_6{ get; set; }
		/// <summary>
		/// 目标拉新7
		/// </summary>
		public Nullable<decimal> new_7{ get; set; }
		/// <summary>
		/// 目标拉新8
		/// </summary>
		public Nullable<decimal> new_8{ get; set; }
		/// <summary>
		/// 目标拉新9
		/// </summary>
		public Nullable<decimal> new_9{ get; set; }
		/// <summary>
		/// 目标拉新10
		/// </summary>
		public Nullable<decimal> new_10{ get; set; }
		/// <summary>
		/// 目标拉新11
		/// </summary>
		public Nullable<decimal> new_11{ get; set; }
		/// <summary>
		/// 目标拉新12
		/// </summary>
		public Nullable<decimal> new_12{ get; set; }
		/// <summary>
		/// 目标拉新13
		/// </summary>
		public Nullable<decimal> new_13{ get; set; }
		/// <summary>
		/// 目标拉新14
		/// </summary>
		public Nullable<decimal> new_14{ get; set; }
		/// <summary>
		/// 目标拉新15
		/// </summary>
		public Nullable<decimal> new_15{ get; set; }
		/// <summary>
		/// 目标拉新16
		/// </summary>
		public Nullable<decimal> new_16{ get; set; }
		/// <summary>
		/// 目标拉新17
		/// </summary>
		public Nullable<decimal> new_17{ get; set; }
		/// <summary>
		/// 目标拉新18
		/// </summary>
		public Nullable<decimal> new_18{ get; set; }
		/// <summary>
		/// 目标拉新19
		/// </summary>
		public Nullable<decimal> new_19{ get; set; }
		/// <summary>
		/// 目标拉新20
		/// </summary>
		public Nullable<decimal> new_20{ get; set; }
		/// <summary>
		/// 目标拉新21
		/// </summary>
		public Nullable<decimal> new_21{ get; set; }
		/// <summary>
		/// 目标拉新22
		/// </summary>
		public Nullable<decimal> new_22{ get; set; }
		/// <summary>
		/// 目标拉新23
		/// </summary>
		public Nullable<decimal> new_23{ get; set; }
		/// <summary>
		/// 目标拉新24
		/// </summary>
		public Nullable<decimal> new_24{ get; set; }
		/// <summary>
		/// 目标拉新25
		/// </summary>
		public Nullable<decimal> new_25{ get; set; }
		/// <summary>
		/// 目标拉新26
		/// </summary>
		public Nullable<decimal> new_26{ get; set; }
		/// <summary>
		/// 目标拉新27
		/// </summary>
		public Nullable<decimal> new_27{ get; set; }
		/// <summary>
		/// 目标拉新28
		/// </summary>
		public Nullable<decimal> new_28{ get; set; }
		/// <summary>
		/// 目标拉新29
		/// </summary>
		public Nullable<decimal> new_29{ get; set; }
		/// <summary>
		/// 目标拉新30
		/// </summary>
		public Nullable<decimal> new_30{ get; set; }
		/// <summary>
		/// 目标拉新31
		/// </summary>
		public Nullable<decimal> new_31{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-日目标-二消数 
			/// </summary>	
			public class _p_jixiao_target_num2 : ModelDbBase
			{    
							public _p_jixiao_target_num2(){}
				public _p_jixiao_target_num2(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.num2_1 =  1m;
														this.num2_2 =  1m;
														this.num2_3 =  1m;
														this.num2_4 =  1m;
														this.num2_5 =  1m;
														this.num2_6 =  1m;
														this.num2_7 =  1m;
														this.num2_8 =  1m;
														this.num2_9 =  1m;
														this.num2_10 =  1m;
														this.num2_11 =  1m;
														this.num2_12 =  1m;
														this.num2_13 =  1m;
														this.num2_14 =  1m;
														this.num2_15 =  1m;
														this.num2_16 =  1m;
														this.num2_17 =  1m;
														this.num2_18 =  1m;
														this.num2_19 =  1m;
														this.num2_20 =  1m;
														this.num2_21 =  1m;
														this.num2_22 =  1m;
														this.num2_23 =  1m;
														this.num2_24 =  1m;
														this.num2_25 =  1m;
														this.num2_26 =  1m;
														this.num2_27 =  1m;
														this.num2_28 =  1m;
														this.num2_29 =  1m;
														this.num2_30 =  1m;
														this.num2_31 =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 目标月份
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 目标音浪1
		/// </summary>
		public Nullable<decimal> num2_1{ get; set; }
		/// <summary>
		/// 目标音浪2
		/// </summary>
		public Nullable<decimal> num2_2{ get; set; }
		/// <summary>
		/// 目标音浪3
		/// </summary>
		public Nullable<decimal> num2_3{ get; set; }
		/// <summary>
		/// 目标音浪4
		/// </summary>
		public Nullable<decimal> num2_4{ get; set; }
		/// <summary>
		/// 目标音浪5
		/// </summary>
		public Nullable<decimal> num2_5{ get; set; }
		/// <summary>
		/// 目标音浪6
		/// </summary>
		public Nullable<decimal> num2_6{ get; set; }
		/// <summary>
		/// 目标音浪7
		/// </summary>
		public Nullable<decimal> num2_7{ get; set; }
		/// <summary>
		/// 目标音浪8
		/// </summary>
		public Nullable<decimal> num2_8{ get; set; }
		/// <summary>
		/// 目标音浪9
		/// </summary>
		public Nullable<decimal> num2_9{ get; set; }
		/// <summary>
		/// 目标音浪10
		/// </summary>
		public Nullable<decimal> num2_10{ get; set; }
		/// <summary>
		/// 目标音浪11
		/// </summary>
		public Nullable<decimal> num2_11{ get; set; }
		/// <summary>
		/// 目标音浪12
		/// </summary>
		public Nullable<decimal> num2_12{ get; set; }
		/// <summary>
		/// 目标音浪13
		/// </summary>
		public Nullable<decimal> num2_13{ get; set; }
		/// <summary>
		/// 目标音浪14
		/// </summary>
		public Nullable<decimal> num2_14{ get; set; }
		/// <summary>
		/// 目标音浪15
		/// </summary>
		public Nullable<decimal> num2_15{ get; set; }
		/// <summary>
		/// 目标音浪16
		/// </summary>
		public Nullable<decimal> num2_16{ get; set; }
		/// <summary>
		/// 目标音浪17
		/// </summary>
		public Nullable<decimal> num2_17{ get; set; }
		/// <summary>
		/// 目标音浪18
		/// </summary>
		public Nullable<decimal> num2_18{ get; set; }
		/// <summary>
		/// 目标音浪19
		/// </summary>
		public Nullable<decimal> num2_19{ get; set; }
		/// <summary>
		/// 目标音浪20
		/// </summary>
		public Nullable<decimal> num2_20{ get; set; }
		/// <summary>
		/// 目标音浪21
		/// </summary>
		public Nullable<decimal> num2_21{ get; set; }
		/// <summary>
		/// 目标音浪22
		/// </summary>
		public Nullable<decimal> num2_22{ get; set; }
		/// <summary>
		/// 目标音浪23
		/// </summary>
		public Nullable<decimal> num2_23{ get; set; }
		/// <summary>
		/// 目标音浪24
		/// </summary>
		public Nullable<decimal> num2_24{ get; set; }
		/// <summary>
		/// 目标音浪25
		/// </summary>
		public Nullable<decimal> num2_25{ get; set; }
		/// <summary>
		/// 目标音浪26
		/// </summary>
		public Nullable<decimal> num2_26{ get; set; }
		/// <summary>
		/// 目标音浪27
		/// </summary>
		public Nullable<decimal> num2_27{ get; set; }
		/// <summary>
		/// 目标音浪28
		/// </summary>
		public Nullable<decimal> num2_28{ get; set; }
		/// <summary>
		/// 目标音浪29
		/// </summary>
		public Nullable<decimal> num2_29{ get; set; }
		/// <summary>
		/// 目标音浪30
		/// </summary>
		public Nullable<decimal> num2_30{ get; set; }
		/// <summary>
		/// 目标音浪31
		/// </summary>
		public Nullable<decimal> num2_31{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-厅目标 
			/// </summary>	
			public class _p_jixiao_target_tg : ModelDbBase
			{    
							public _p_jixiao_target_tg(){}
				public _p_jixiao_target_tg(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.amount =  1m;
														this.amount_target_1 =  1m;
														this.amount_target_2 =  1m;
														this.amount_target_3 =  1m;
														this.new_num =  1;
														this.amount_2 =  1m;
														this.num_2 =  1;
														this.contact_num =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 目标月份
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 目标音浪
		/// </summary>
		public Nullable<decimal> amount{ get; set; }
		/// <summary>
		/// 目标音浪-阶段一
		/// </summary>
		public Nullable<decimal> amount_target_1{ get; set; }
		/// <summary>
		/// 目标音浪-阶段二
		/// </summary>
		public Nullable<decimal> amount_target_2{ get; set; }
		/// <summary>
		/// 目标音浪-阶段三
		/// </summary>
		public Nullable<decimal> amount_target_3{ get; set; }
		/// <summary>
		/// 目标拉新
		/// </summary>
		public Nullable<int> new_num{ get; set; }
		/// <summary>
		/// 目标二消
		/// </summary>
		public Nullable<decimal> amount_2{ get; set; }
		/// <summary>
		/// 目标二消数
		/// </summary>
		public Nullable<int> num_2{ get; set; }
		/// <summary>
		/// 目标建联
		/// </summary>
		public Nullable<int> contact_num{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-日目标-首消 
			/// </summary>	
			public class _p_jixiao_tgtarget_amount : ModelDbBase
			{    
							public _p_jixiao_tgtarget_amount(){}
				public _p_jixiao_tgtarget_amount(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.amount_1 =  1m;
														this.amount_2 =  1m;
														this.amount_3 =  1m;
														this.amount_4 =  1m;
														this.amount_5 =  1m;
														this.amount_6 =  1m;
														this.amount_7 =  1m;
														this.amount_8 =  1m;
														this.amount_9 =  1m;
														this.amount_10 =  1m;
														this.amount_11 =  1m;
														this.amount_12 =  1m;
														this.amount_13 =  1m;
														this.amount_14 =  1m;
														this.amount_15 =  1m;
														this.amount_16 =  1m;
														this.amount_17 =  1m;
														this.amount_18 =  1m;
														this.amount_19 =  1m;
														this.amount_20 =  1m;
														this.amount_21 =  1m;
														this.amount_22 =  1m;
														this.amount_23 =  1m;
														this.amount_24 =  1m;
														this.amount_25 =  1m;
														this.amount_26 =  1m;
														this.amount_27 =  1m;
														this.amount_28 =  1m;
														this.amount_29 =  1m;
														this.amount_30 =  1m;
														this.amount_31 =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 目标月份
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 目标音浪1
		/// </summary>
		public Nullable<decimal> amount_1{ get; set; }
		/// <summary>
		/// 目标音浪2
		/// </summary>
		public Nullable<decimal> amount_2{ get; set; }
		/// <summary>
		/// 目标音浪3
		/// </summary>
		public Nullable<decimal> amount_3{ get; set; }
		/// <summary>
		/// 目标音浪4
		/// </summary>
		public Nullable<decimal> amount_4{ get; set; }
		/// <summary>
		/// 目标音浪5
		/// </summary>
		public Nullable<decimal> amount_5{ get; set; }
		/// <summary>
		/// 目标音浪6
		/// </summary>
		public Nullable<decimal> amount_6{ get; set; }
		/// <summary>
		/// 目标音浪7
		/// </summary>
		public Nullable<decimal> amount_7{ get; set; }
		/// <summary>
		/// 目标音浪8
		/// </summary>
		public Nullable<decimal> amount_8{ get; set; }
		/// <summary>
		/// 目标音浪9
		/// </summary>
		public Nullable<decimal> amount_9{ get; set; }
		/// <summary>
		/// 目标音浪10
		/// </summary>
		public Nullable<decimal> amount_10{ get; set; }
		/// <summary>
		/// 目标音浪11
		/// </summary>
		public Nullable<decimal> amount_11{ get; set; }
		/// <summary>
		/// 目标音浪12
		/// </summary>
		public Nullable<decimal> amount_12{ get; set; }
		/// <summary>
		/// 目标音浪13
		/// </summary>
		public Nullable<decimal> amount_13{ get; set; }
		/// <summary>
		/// 目标音浪14
		/// </summary>
		public Nullable<decimal> amount_14{ get; set; }
		/// <summary>
		/// 目标音浪15
		/// </summary>
		public Nullable<decimal> amount_15{ get; set; }
		/// <summary>
		/// 目标音浪16
		/// </summary>
		public Nullable<decimal> amount_16{ get; set; }
		/// <summary>
		/// 目标音浪17
		/// </summary>
		public Nullable<decimal> amount_17{ get; set; }
		/// <summary>
		/// 目标音浪18
		/// </summary>
		public Nullable<decimal> amount_18{ get; set; }
		/// <summary>
		/// 目标音浪19
		/// </summary>
		public Nullable<decimal> amount_19{ get; set; }
		/// <summary>
		/// 目标音浪20
		/// </summary>
		public Nullable<decimal> amount_20{ get; set; }
		/// <summary>
		/// 目标音浪21
		/// </summary>
		public Nullable<decimal> amount_21{ get; set; }
		/// <summary>
		/// 目标音浪22
		/// </summary>
		public Nullable<decimal> amount_22{ get; set; }
		/// <summary>
		/// 目标音浪23
		/// </summary>
		public Nullable<decimal> amount_23{ get; set; }
		/// <summary>
		/// 目标音浪24
		/// </summary>
		public Nullable<decimal> amount_24{ get; set; }
		/// <summary>
		/// 目标音浪25
		/// </summary>
		public Nullable<decimal> amount_25{ get; set; }
		/// <summary>
		/// 目标音浪26
		/// </summary>
		public Nullable<decimal> amount_26{ get; set; }
		/// <summary>
		/// 目标音浪27
		/// </summary>
		public Nullable<decimal> amount_27{ get; set; }
		/// <summary>
		/// 目标音浪28
		/// </summary>
		public Nullable<decimal> amount_28{ get; set; }
		/// <summary>
		/// 目标音浪29
		/// </summary>
		public Nullable<decimal> amount_29{ get; set; }
		/// <summary>
		/// 目标音浪30
		/// </summary>
		public Nullable<decimal> amount_30{ get; set; }
		/// <summary>
		/// 目标音浪31
		/// </summary>
		public Nullable<decimal> amount_31{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-日目标-建联 
			/// </summary>	
			public class _p_jixiao_tgtarget_contact : ModelDbBase
			{    
							public _p_jixiao_tgtarget_contact(){}
				public _p_jixiao_tgtarget_contact(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.contact_1 =  1m;
														this.contact_2 =  1m;
														this.contact_3 =  1m;
														this.contact_4 =  1m;
														this.contact_5 =  1m;
														this.contact_6 =  1m;
														this.contact_7 =  1m;
														this.contact_8 =  1m;
														this.contact_9 =  1m;
														this.contact_10 =  1m;
														this.contact_11 =  1m;
														this.contact_12 =  1m;
														this.contact_13 =  1m;
														this.contact_14 =  1m;
														this.contact_15 =  1m;
														this.contact_16 =  1m;
														this.contact_17 =  1m;
														this.contact_18 =  1m;
														this.contact_19 =  1m;
														this.contact_20 =  1m;
														this.contact_21 =  1m;
														this.contact_22 =  1m;
														this.contact_23 =  1m;
														this.contact_24 =  1m;
														this.contact_25 =  1m;
														this.contact_26 =  1m;
														this.contact_27 =  1m;
														this.contact_28 =  1m;
														this.contact_29 =  1m;
														this.contact_30 =  1m;
														this.contact_31 =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 目标月份
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 目标音浪1
		/// </summary>
		public Nullable<decimal> contact_1{ get; set; }
		/// <summary>
		/// 目标音浪2
		/// </summary>
		public Nullable<decimal> contact_2{ get; set; }
		/// <summary>
		/// 目标音浪3
		/// </summary>
		public Nullable<decimal> contact_3{ get; set; }
		/// <summary>
		/// 目标音浪4
		/// </summary>
		public Nullable<decimal> contact_4{ get; set; }
		/// <summary>
		/// 目标音浪5
		/// </summary>
		public Nullable<decimal> contact_5{ get; set; }
		/// <summary>
		/// 目标音浪6
		/// </summary>
		public Nullable<decimal> contact_6{ get; set; }
		/// <summary>
		/// 目标音浪7
		/// </summary>
		public Nullable<decimal> contact_7{ get; set; }
		/// <summary>
		/// 目标音浪8
		/// </summary>
		public Nullable<decimal> contact_8{ get; set; }
		/// <summary>
		/// 目标音浪9
		/// </summary>
		public Nullable<decimal> contact_9{ get; set; }
		/// <summary>
		/// 目标音浪10
		/// </summary>
		public Nullable<decimal> contact_10{ get; set; }
		/// <summary>
		/// 目标音浪11
		/// </summary>
		public Nullable<decimal> contact_11{ get; set; }
		/// <summary>
		/// 目标音浪12
		/// </summary>
		public Nullable<decimal> contact_12{ get; set; }
		/// <summary>
		/// 目标音浪13
		/// </summary>
		public Nullable<decimal> contact_13{ get; set; }
		/// <summary>
		/// 目标音浪14
		/// </summary>
		public Nullable<decimal> contact_14{ get; set; }
		/// <summary>
		/// 目标音浪15
		/// </summary>
		public Nullable<decimal> contact_15{ get; set; }
		/// <summary>
		/// 目标音浪16
		/// </summary>
		public Nullable<decimal> contact_16{ get; set; }
		/// <summary>
		/// 目标音浪17
		/// </summary>
		public Nullable<decimal> contact_17{ get; set; }
		/// <summary>
		/// 目标音浪18
		/// </summary>
		public Nullable<decimal> contact_18{ get; set; }
		/// <summary>
		/// 目标音浪19
		/// </summary>
		public Nullable<decimal> contact_19{ get; set; }
		/// <summary>
		/// 目标音浪20
		/// </summary>
		public Nullable<decimal> contact_20{ get; set; }
		/// <summary>
		/// 目标音浪21
		/// </summary>
		public Nullable<decimal> contact_21{ get; set; }
		/// <summary>
		/// 目标音浪22
		/// </summary>
		public Nullable<decimal> contact_22{ get; set; }
		/// <summary>
		/// 目标音浪23
		/// </summary>
		public Nullable<decimal> contact_23{ get; set; }
		/// <summary>
		/// 目标音浪24
		/// </summary>
		public Nullable<decimal> contact_24{ get; set; }
		/// <summary>
		/// 目标音浪25
		/// </summary>
		public Nullable<decimal> contact_25{ get; set; }
		/// <summary>
		/// 目标音浪26
		/// </summary>
		public Nullable<decimal> contact_26{ get; set; }
		/// <summary>
		/// 目标音浪27
		/// </summary>
		public Nullable<decimal> contact_27{ get; set; }
		/// <summary>
		/// 目标音浪28
		/// </summary>
		public Nullable<decimal> contact_28{ get; set; }
		/// <summary>
		/// 目标音浪29
		/// </summary>
		public Nullable<decimal> contact_29{ get; set; }
		/// <summary>
		/// 目标音浪30
		/// </summary>
		public Nullable<decimal> contact_30{ get; set; }
		/// <summary>
		/// 目标音浪31
		/// </summary>
		public Nullable<decimal> contact_31{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-日目标-拉新 
			/// </summary>	
			public class _p_jixiao_tgtarget_new : ModelDbBase
			{    
							public _p_jixiao_tgtarget_new(){}
				public _p_jixiao_tgtarget_new(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.new_1 =  1m;
														this.new_2 =  1m;
														this.new_3 =  1m;
														this.new_4 =  1m;
														this.new_5 =  1m;
														this.new_6 =  1m;
														this.new_7 =  1m;
														this.new_8 =  1m;
														this.new_9 =  1m;
														this.new_10 =  1m;
														this.new_11 =  1m;
														this.new_12 =  1m;
														this.new_13 =  1m;
														this.new_14 =  1m;
														this.new_15 =  1m;
														this.new_16 =  1m;
														this.new_17 =  1m;
														this.new_18 =  1m;
														this.new_19 =  1m;
														this.new_20 =  1m;
														this.new_21 =  1m;
														this.new_22 =  1m;
														this.new_23 =  1m;
														this.new_24 =  1m;
														this.new_25 =  1m;
														this.new_26 =  1m;
														this.new_27 =  1m;
														this.new_28 =  1m;
														this.new_29 =  1m;
														this.new_30 =  1m;
														this.new_31 =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 目标月份
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 目标拉新1
		/// </summary>
		public Nullable<decimal> new_1{ get; set; }
		/// <summary>
		/// 目标拉新2
		/// </summary>
		public Nullable<decimal> new_2{ get; set; }
		/// <summary>
		/// 目标拉新3
		/// </summary>
		public Nullable<decimal> new_3{ get; set; }
		/// <summary>
		/// 目标拉新4
		/// </summary>
		public Nullable<decimal> new_4{ get; set; }
		/// <summary>
		/// 目标拉新5
		/// </summary>
		public Nullable<decimal> new_5{ get; set; }
		/// <summary>
		/// 目标拉新6
		/// </summary>
		public Nullable<decimal> new_6{ get; set; }
		/// <summary>
		/// 目标拉新7
		/// </summary>
		public Nullable<decimal> new_7{ get; set; }
		/// <summary>
		/// 目标拉新8
		/// </summary>
		public Nullable<decimal> new_8{ get; set; }
		/// <summary>
		/// 目标拉新9
		/// </summary>
		public Nullable<decimal> new_9{ get; set; }
		/// <summary>
		/// 目标拉新10
		/// </summary>
		public Nullable<decimal> new_10{ get; set; }
		/// <summary>
		/// 目标拉新11
		/// </summary>
		public Nullable<decimal> new_11{ get; set; }
		/// <summary>
		/// 目标拉新12
		/// </summary>
		public Nullable<decimal> new_12{ get; set; }
		/// <summary>
		/// 目标拉新13
		/// </summary>
		public Nullable<decimal> new_13{ get; set; }
		/// <summary>
		/// 目标拉新14
		/// </summary>
		public Nullable<decimal> new_14{ get; set; }
		/// <summary>
		/// 目标拉新15
		/// </summary>
		public Nullable<decimal> new_15{ get; set; }
		/// <summary>
		/// 目标拉新16
		/// </summary>
		public Nullable<decimal> new_16{ get; set; }
		/// <summary>
		/// 目标拉新17
		/// </summary>
		public Nullable<decimal> new_17{ get; set; }
		/// <summary>
		/// 目标拉新18
		/// </summary>
		public Nullable<decimal> new_18{ get; set; }
		/// <summary>
		/// 目标拉新19
		/// </summary>
		public Nullable<decimal> new_19{ get; set; }
		/// <summary>
		/// 目标拉新20
		/// </summary>
		public Nullable<decimal> new_20{ get; set; }
		/// <summary>
		/// 目标拉新21
		/// </summary>
		public Nullable<decimal> new_21{ get; set; }
		/// <summary>
		/// 目标拉新22
		/// </summary>
		public Nullable<decimal> new_22{ get; set; }
		/// <summary>
		/// 目标拉新23
		/// </summary>
		public Nullable<decimal> new_23{ get; set; }
		/// <summary>
		/// 目标拉新24
		/// </summary>
		public Nullable<decimal> new_24{ get; set; }
		/// <summary>
		/// 目标拉新25
		/// </summary>
		public Nullable<decimal> new_25{ get; set; }
		/// <summary>
		/// 目标拉新26
		/// </summary>
		public Nullable<decimal> new_26{ get; set; }
		/// <summary>
		/// 目标拉新27
		/// </summary>
		public Nullable<decimal> new_27{ get; set; }
		/// <summary>
		/// 目标拉新28
		/// </summary>
		public Nullable<decimal> new_28{ get; set; }
		/// <summary>
		/// 目标拉新29
		/// </summary>
		public Nullable<decimal> new_29{ get; set; }
		/// <summary>
		/// 目标拉新30
		/// </summary>
		public Nullable<decimal> new_30{ get; set; }
		/// <summary>
		/// 目标拉新31
		/// </summary>
		public Nullable<decimal> new_31{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-日目标-二消数 
			/// </summary>	
			public class _p_jixiao_tgtarget_num2 : ModelDbBase
			{    
							public _p_jixiao_tgtarget_num2(){}
				public _p_jixiao_tgtarget_num2(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.num2_1 =  1m;
														this.num2_2 =  1m;
														this.num2_3 =  1m;
														this.num2_4 =  1m;
														this.num2_5 =  1m;
														this.num2_6 =  1m;
														this.num2_7 =  1m;
														this.num2_8 =  1m;
														this.num2_9 =  1m;
														this.num2_10 =  1m;
														this.num2_11 =  1m;
														this.num2_12 =  1m;
														this.num2_13 =  1m;
														this.num2_14 =  1m;
														this.num2_15 =  1m;
														this.num2_16 =  1m;
														this.num2_17 =  1m;
														this.num2_18 =  1m;
														this.num2_19 =  1m;
														this.num2_20 =  1m;
														this.num2_21 =  1m;
														this.num2_22 =  1m;
														this.num2_23 =  1m;
														this.num2_24 =  1m;
														this.num2_25 =  1m;
														this.num2_26 =  1m;
														this.num2_27 =  1m;
														this.num2_28 =  1m;
														this.num2_29 =  1m;
														this.num2_30 =  1m;
														this.num2_31 =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 目标月份
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 目标音浪1
		/// </summary>
		public Nullable<decimal> num2_1{ get; set; }
		/// <summary>
		/// 目标音浪2
		/// </summary>
		public Nullable<decimal> num2_2{ get; set; }
		/// <summary>
		/// 目标音浪3
		/// </summary>
		public Nullable<decimal> num2_3{ get; set; }
		/// <summary>
		/// 目标音浪4
		/// </summary>
		public Nullable<decimal> num2_4{ get; set; }
		/// <summary>
		/// 目标音浪5
		/// </summary>
		public Nullable<decimal> num2_5{ get; set; }
		/// <summary>
		/// 目标音浪6
		/// </summary>
		public Nullable<decimal> num2_6{ get; set; }
		/// <summary>
		/// 目标音浪7
		/// </summary>
		public Nullable<decimal> num2_7{ get; set; }
		/// <summary>
		/// 目标音浪8
		/// </summary>
		public Nullable<decimal> num2_8{ get; set; }
		/// <summary>
		/// 目标音浪9
		/// </summary>
		public Nullable<decimal> num2_9{ get; set; }
		/// <summary>
		/// 目标音浪10
		/// </summary>
		public Nullable<decimal> num2_10{ get; set; }
		/// <summary>
		/// 目标音浪11
		/// </summary>
		public Nullable<decimal> num2_11{ get; set; }
		/// <summary>
		/// 目标音浪12
		/// </summary>
		public Nullable<decimal> num2_12{ get; set; }
		/// <summary>
		/// 目标音浪13
		/// </summary>
		public Nullable<decimal> num2_13{ get; set; }
		/// <summary>
		/// 目标音浪14
		/// </summary>
		public Nullable<decimal> num2_14{ get; set; }
		/// <summary>
		/// 目标音浪15
		/// </summary>
		public Nullable<decimal> num2_15{ get; set; }
		/// <summary>
		/// 目标音浪16
		/// </summary>
		public Nullable<decimal> num2_16{ get; set; }
		/// <summary>
		/// 目标音浪17
		/// </summary>
		public Nullable<decimal> num2_17{ get; set; }
		/// <summary>
		/// 目标音浪18
		/// </summary>
		public Nullable<decimal> num2_18{ get; set; }
		/// <summary>
		/// 目标音浪19
		/// </summary>
		public Nullable<decimal> num2_19{ get; set; }
		/// <summary>
		/// 目标音浪20
		/// </summary>
		public Nullable<decimal> num2_20{ get; set; }
		/// <summary>
		/// 目标音浪21
		/// </summary>
		public Nullable<decimal> num2_21{ get; set; }
		/// <summary>
		/// 目标音浪22
		/// </summary>
		public Nullable<decimal> num2_22{ get; set; }
		/// <summary>
		/// 目标音浪23
		/// </summary>
		public Nullable<decimal> num2_23{ get; set; }
		/// <summary>
		/// 目标音浪24
		/// </summary>
		public Nullable<decimal> num2_24{ get; set; }
		/// <summary>
		/// 目标音浪25
		/// </summary>
		public Nullable<decimal> num2_25{ get; set; }
		/// <summary>
		/// 目标音浪26
		/// </summary>
		public Nullable<decimal> num2_26{ get; set; }
		/// <summary>
		/// 目标音浪27
		/// </summary>
		public Nullable<decimal> num2_27{ get; set; }
		/// <summary>
		/// 目标音浪28
		/// </summary>
		public Nullable<decimal> num2_28{ get; set; }
		/// <summary>
		/// 目标音浪29
		/// </summary>
		public Nullable<decimal> num2_29{ get; set; }
		/// <summary>
		/// 目标音浪30
		/// </summary>
		public Nullable<decimal> num2_30{ get; set; }
		/// <summary>
		/// 目标音浪31
		/// </summary>
		public Nullable<decimal> num2_31{ get; set; }
			}
	    			/// <summary>
			/// 表实体-资产-资产表 
			/// </summary>	
			public class asset : ModelDbBase
			{    
							public asset(){}
				public asset(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.p_category_id =  1;
														this.category_id =  1;
														this.price =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 一级类别id
		/// </summary>
		public Nullable<int> p_category_id{ get; set; }
		/// <summary>
		/// 类别id
		/// </summary>
		public Nullable<int> category_id{ get; set; }
		/// <summary>
		/// 资产名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 规格型号
		/// </summary>
		public string spec{ get; set; }
		/// <summary>
		/// 资产编号
		/// </summary>
		public string asset_sn{ get; set; }
		/// <summary>
		/// 品牌
		/// </summary>
		public string brand{ get; set; }
		/// <summary>
		/// </summary>
		/// <summary>
		/// 登记日期
		/// </summary>
		public Nullable<DateTime> on_date{ get; set; }
		/// <summary>
		/// 资产价值
		/// </summary>
		public Nullable<decimal> price{ get; set; }
		/// <summary>
		/// 采购人编号
		/// </summary>
		public string cj_user_sn{ get; set; }
		/// <summary>
		/// 购买平台
		/// </summary>
		public string purchase_platform{ get; set; }
		/// <summary>
		/// 使用人编号
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 图片url
		/// </summary>
		public string picture{ get; set; }
		/// <summary>
		/// 资产状态#enum:空闲=0;在用=1;待入=2
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			空闲=0,
			在用=1,
			待入=2,
		}
		/// <summary>
		/// 删除状态#enum:未删除=0;已删除=1
		/// </summary>
		public Nullable<sbyte> is_deleted{ get; set; }
		public enum is_deleted_enum {
			未删除=0,
			已删除=1,
		}
			}
	    			/// <summary>
			/// 表实体-资产-分类 
			/// </summary>	
			public class asset_category : ModelDbBase
			{    
							public asset_category(){}
				public asset_category(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.parent_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 父级类别
		/// </summary>
		public Nullable<int> parent_id{ get; set; }
		/// <summary>
		/// 类别名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 排序号
		/// </summary>
		public Nullable<int> sort{ get; set; }
		/// <summary>
		/// 是否默认
		/// </summary>
		public Nullable<sbyte> is_default{ get; set; }
		/// <summary>
		/// 所属中台
		/// </summary>
		public string zt_user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-资产-入库信息 
			/// </summary>	
			public class asset_in : ModelDbBase
			{    
							public asset_in(){}
				public asset_in(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.num =  1;
												}
				}
			/// <summary>
		/// 
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 入库单号
		/// </summary>
		public string in_sn{ get; set; }
		/// <summary>
		/// 领用人编号
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 操作类型#enum:采购入库=1;退库入库=2
		/// </summary>
		public Nullable<sbyte> op_type{ get; set; }
		public enum op_type_enum {
			采购入库=1,
			退库入库=2,
		}
		/// <summary>
		/// 操作类型备注
		/// </summary>
		public string op_note{ get; set; }
		/// <summary>
		/// 入库数量
		/// </summary>
		public Nullable<int> num{ get; set; }
		/// <summary>
		/// 所属基地
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 审核事由
		/// </summary>
		public string cause{ get; set; }
			}
	    			/// <summary>
			/// 表实体-资产-入库明细表 
			/// </summary>	
			public class asset_in_item : ModelDbBase
			{    
							public asset_in_item(){}
				public asset_in_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 入库编号
		/// </summary>
		public string in_sn{ get; set; }
		/// <summary>
		/// 资产编号
		/// </summary>
		public string asset_sn{ get; set; }
		/// <summary>
		/// 资产持有人编号
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 入库状态#enum:等待入库=0;入库成功=1;入库失败=2
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			等待入库=0,
			入库成功=1,
			入库失败=2,
		}
		/// <summary>
		/// 审核事由
		/// </summary>
		public string cause{ get; set; }
		/// <summary>
		/// 所属基地
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 约定入库时间
		/// </summary>
		public Nullable<DateTime> plan_time{ get; set; }
		/// <summary>
		/// 实际入库时间
		/// </summary>
		public Nullable<DateTime> in_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-资产-转移信息 
			/// </summary>	
			public class asset_move : ModelDbBase
			{    
							public asset_move(){}
				public asset_move(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.staff_id_before =  1;
														this.organize_id_before =  1;
														this.staff_id_after =  1;
														this.organize_id_after =  1;
														this.asset_id =  1;
														this.parent_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 转移前领用人编号
		/// </summary>
		public string user_sn_before{ get; set; }
		/// <summary>
		/// 转移前员工id
		/// </summary>
		public Nullable<int> staff_id_before{ get; set; }
		/// <summary>
		/// 转移前所属部门id
		/// </summary>
		public Nullable<int> organize_id_before{ get; set; }
		/// <summary>
		/// 转移后领用人编号
		/// </summary>
		public string user_sn_after{ get; set; }
		/// <summary>
		/// 转移后领用人的员工id
		/// </summary>
		public Nullable<int> staff_id_after{ get; set; }
		/// <summary>
		/// 转移后领用人的所属部门id
		/// </summary>
		public Nullable<int> organize_id_after{ get; set; }
		/// <summary>
		/// 转移编号
		/// </summary>
		public string move_sn{ get; set; }
		/// <summary>
		/// 资产id
		/// </summary>
		public Nullable<int> asset_id{ get; set; }
		/// <summary>
		/// 转移事由
		/// </summary>
		public string cause{ get; set; }
		/// <summary>
		/// 所属基地
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 转移日期
		/// </summary>
		public Nullable<DateTime> ac_date{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Nullable<int> parent_id{ get; set; }
			}
	    			/// <summary>
			/// 表实体-资产-转移明细表 
			/// </summary>	
			public class asset_move_item : ModelDbBase
			{    
							public asset_move_item(){}
				public asset_move_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.category_id =  1;
												}
				}
			/// <summary>
		/// 
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 类别id
		/// </summary>
		public Nullable<int> category_id{ get; set; }
		/// <summary>
		/// 资产名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 规格型号
		/// </summary>
		public string spec{ get; set; }
		/// <summary>
		/// 品牌
		/// </summary>
		public string brand{ get; set; }
		/// <summary>
		/// 资产持有人编号
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 资产编号
		/// </summary>
		public string asset_sn{ get; set; }
		/// <summary>
		/// 转移编号
		/// </summary>
		public string move_sn{ get; set; }
		/// <summary>
		/// 转移状态;0=等待转移;1=转移成功;2=转移失败
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		/// <summary>
		/// 审核事由
		/// </summary>
		public string cause{ get; set; }
		/// <summary>
		/// 所属基地
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 约定转移时间
		/// </summary>
		public Nullable<DateTime> plan_time{ get; set; }
		/// <summary>
		/// 实际转移时间
		/// </summary>
		public Nullable<DateTime> out_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-资产-出库表 
			/// </summary>	
			public class asset_out : ModelDbBase
			{    
							public asset_out(){}
				public asset_out(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.num =  1;
												}
				}
			/// <summary>
		/// 
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 出库编号
		/// </summary>
		public string out_sn{ get; set; }
		/// <summary>
		/// 领用人编号
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 所属基地
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 出库数量
		/// </summary>
		public Nullable<int> num{ get; set; }
		/// <summary>
		/// 操作类型#enum:派发出库=1
		/// </summary>
		public Nullable<sbyte> op_type{ get; set; }
		public enum op_type_enum {
			派发出库=1,
		}
		/// <summary>
		/// 操作类型备注
		/// </summary>
		public string op_note{ get; set; }
			}
	    			/// <summary>
			/// 表实体-资产-出库明细表 
			/// </summary>	
			public class asset_out_item : ModelDbBase
			{    
							public asset_out_item(){}
				public asset_out_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 出库编号
		/// </summary>
		public string out_sn{ get; set; }
		/// <summary>
		/// 领用人编号
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 资产编号
		/// </summary>
		public string asset_sn{ get; set; }
		/// <summary>
		/// 入库状态#enum:等待出库=0;出库成功=1;出库失败=2
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			等待出库=0,
			出库成功=1,
			出库失败=2,
		}
		/// <summary>
		/// 审核事由
		/// </summary>
		public string cause{ get; set; }
		/// <summary>
		/// 所属基地
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 约定出库时间
		/// </summary>
		public Nullable<DateTime> plan_time{ get; set; }
		/// <summary>
		/// 实际出库时间
		/// </summary>
		public Nullable<DateTime> out_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-资产-盘点 
			/// </summary>	
			public class asset_stock : ModelDbBase
			{    
							public asset_stock(){}
				public asset_stock(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 盘点编号
		/// </summary>
		public string stock_sn{ get; set; }
		/// <summary>
		/// 盘点单名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 入库开始时间
		/// </summary>
		public Nullable<DateTime> in_s_time{ get; set; }
		/// <summary>
		/// 入库结束时间
		/// </summary>
		public Nullable<DateTime> in_e_time{ get; set; }
		/// <summary>
		/// 筛选使用人范围
		/// </summary>
		public string user_sns{ get; set; }
		/// <summary>
		/// 所属基地
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 盘点状态#enum:盘点中=0;已完成=1;已取消=2
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			盘点中=0,
			已完成=1,
			已取消=2,
		}
		/// <summary>
		/// 计划盘点开始时间
		/// </summary>
		public Nullable<DateTime> plan_s_time{ get; set; }
		/// <summary>
		/// 计划盘点结束时间
		/// </summary>
		public Nullable<DateTime> plan_e_time{ get; set; }
		/// <summary>
		/// 实际完成时间
		/// </summary>
		public Nullable<DateTime> real_e_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-资产-盘点-明细 
			/// </summary>	
			public class asset_stock_item : ModelDbBase
			{    
							public asset_stock_item(){}
				public asset_stock_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 盘点编号
		/// </summary>
		public string stock_sn{ get; set; }
		/// <summary>
		/// 资产编号
		/// </summary>
		public string asset_sn{ get; set; }
		/// <summary>
		/// 盘点结果#enum:待盘点=0;正常=1
		/// </summary>
		public Nullable<sbyte> p_status{ get; set; }
		public enum p_status_enum {
			待盘点=0,
			正常=1,
		}
		/// <summary>
		/// 盘点后使用人编号
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 资产状态#enum:空闲=0;在用=1;待入=2
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			空闲=0,
			在用=1,
			待入=2,
		}
		/// <summary>
		/// 所属基地
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 盘点备注
		/// </summary>
		public string cause{ get; set; }
		/// <summary>
		/// 计划盘点时间
		/// </summary>
		public Nullable<DateTime> plan_time{ get; set; }
		/// <summary>
		/// 实际完成时间
		/// </summary>
		public Nullable<DateTime> end_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-crm-客户基础信息 
			/// </summary>	
			public class crm_base : ModelDbBase
			{    
							public crm_base(){}
				public crm_base(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.type_id =  1;
														this.grade_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 客户编号
		/// </summary>
		public string crm_sn{ get; set; }
		/// <summary>
		/// 所属用户
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 上级所属用户（没有时留空）
		/// </summary>
		public string p_user_sn{ get; set; }
		/// <summary>
		/// 上上级所属用户（没有时留空）
		/// </summary>
		public string pp_user_sn{ get; set; }
		/// <summary>
		/// 类型id
		/// </summary>
		public Nullable<int> type_id{ get; set; }
		/// <summary>
		/// 客户等级
		/// </summary>
		public Nullable<int> grade_id{ get; set; }
		/// <summary>
		/// 上次联系时间
		/// </summary>
		public Nullable<DateTime> last_time{ get; set; }
		/// <summary>
		/// 下次联系时间
		/// </summary>
		public Nullable<DateTime> next_time{ get; set; }
		/// <summary>
		/// 客户名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 微信openid
		/// </summary>
		public string weixin_id{ get; set; }
		/// <summary>
		/// 微信号
		/// </summary>
		public string weixin{ get; set; }
			}
	    			/// <summary>
			/// 表实体-crm-等级 
			/// </summary>	
			public class crm_grade : ModelDbBase
			{    
							public crm_grade(){}
				public crm_grade(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.type_id =  1;
														this.level =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 类型id
		/// </summary>
		public Nullable<int> type_id{ get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 升降级权重，小到大代表升级
		/// </summary>
		public Nullable<int> level{ get; set; }
		/// <summary>
		/// 排序号，越小越靠前
		/// </summary>
		public Nullable<int> sort{ get; set; }
			}
	    			/// <summary>
			/// 表实体-crm-等级-变更日志 
			/// </summary>	
			public class crm_grade_log : ModelDbBase
			{    
							public crm_grade_log(){}
				public crm_grade_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.o_grade_id =  1;
														this.n_grade_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 客户sn
		/// </summary>
		public string crm_sn{ get; set; }
		/// <summary>
		/// 所属用户
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 上级所属用户（没有时留空）
		/// </summary>
		public string p_user_sn{ get; set; }
		/// <summary>
		/// 上上级所属用户（没有时留空）
		/// </summary>
		public string pp_user_sn{ get; set; }
		/// <summary>
		/// 升降类型#enum:新增=0;升级=1;降级=2
		/// </summary>
		public Nullable<sbyte> c_type{ get; set; }
		public enum c_type_enum {
			新增=0,
			升级=1,
			降级=2,
		}
		/// <summary>
		/// 旧等级id
		/// </summary>
		public Nullable<int> o_grade_id{ get; set; }
		/// <summary>
		/// 新等级id
		/// </summary>
		public Nullable<int> n_grade_id{ get; set; }
		/// <summary>
		/// 变更原因备注
		/// </summary>
		public string content{ get; set; }
			}
	    			/// <summary>
			/// 表实体-crm-跟进日志 
			/// </summary>	
			public class crm_log : ModelDbBase
			{    
							public crm_log(){}
				public crm_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 客户sn
		/// </summary>
		public string crm_sn{ get; set; }
		/// <summary>
		/// 所属用户
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 联系情况备注
		/// </summary>
		public string content{ get; set; }
			}
	    			/// <summary>
			/// 表实体-crm-类型 
			/// </summary>	
			public class crm_type : ModelDbBase
			{    
							public crm_type(){}
				public crm_type(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string name{ get; set; }
			}
	    			/// <summary>
			/// 表实体-打卡 
			/// </summary>	
			public class daka : ModelDbBase
			{    
							public daka(){}
				public daka(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 项目名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 排序号
		/// </summary>
		public Nullable<int> sort{ get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
			}
	    			/// <summary>
			/// 表实体-打卡 
			/// </summary>	
			public class daka_log : ModelDbBase
			{    
							public daka_log(){}
				public daka_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.daka_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 项目id
		/// </summary>
		public Nullable<int> daka_id{ get; set; }
		/// <summary>
		/// 打卡日期
		/// </summary>
		public Nullable<DateTime> d_date{ get; set; }
		/// <summary>
		/// 厅管sn
		/// </summary>
		public string tger_sn{ get; set; }
		/// <summary>
		/// 所属运营
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		///  是否完成:1=已完成;2=未完成
		/// </summary>
		public Nullable<sbyte> is_finish{ get; set; }
			}
	    			/// <summary>
			/// 表实体-核心数据-每日-厅 
			/// </summary>	
			public class dataanalysis_coredata_ting : ModelDbBase
			{    
							public dataanalysis_coredata_ting(){}
				public dataanalysis_coredata_ting(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.zb_count =  1;
														this.gear =  1;
														this.liveHour =  1m;
														this.visitor =  1;
														this.hourly_visitors =  1;
														this.live_rate =  1m;
														this.entry_rate =  1m;
														this.interactive_user_count =  1;
														this.paying_user_total =  1;
														this.new_paying_user_count =  1;
														this.avg_second_consumption =  1;
														this.user_count_type_a =  1;
														this.user_count_type_b =  1;
														this.user_count_type_c =  1;
														this.user_count_type_a_last_5d =  1;
														this.user_count_type_a_last_10d =  1;
														this.user_count_type_a_last_15d =  1;
														this.old_user_count_last_3d =  1;
														this.old_user_count_last_5d =  1;
														this.old_user_count_last_7d =  1;
														this.old_user_count_last_15d =  1;
														this.upgraded_to_a_count_last_5d =  1;
														this.totalFanTickets =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// anchorID
		/// </summary>
		public string dou_UID{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 主播人数
		/// </summary>
		public Nullable<int> zb_count{ get; set; }
		/// <summary>
		/// 档位数
		/// </summary>
		public Nullable<int> gear{ get; set; }
		/// <summary>
		/// 直播时长（时）
		/// </summary>
		public Nullable<decimal> liveHour{ get; set; }
		/// <summary>
		/// 访客数
		/// </summary>
		public Nullable<int> visitor{ get; set; }
		/// <summary>
		/// 每小时访客数
		/// </summary>
		public Nullable<int> hourly_visitors{ get; set; }
		/// <summary>
		/// 直播推荐占比
		/// </summary>
		public Nullable<decimal> live_rate{ get; set; }
		/// <summary>
		/// 进入率
		/// </summary>
		public Nullable<decimal> entry_rate{ get; set; }
		/// <summary>
		/// 互动人数
		/// </summary>
		public Nullable<int> interactive_user_count{ get; set; }
		/// <summary>
		/// 付费用户总数
		/// </summary>
		public Nullable<int> paying_user_total{ get; set; }
		/// <summary>
		/// 新付费用户数
		/// </summary>
		public Nullable<int> new_paying_user_count{ get; set; }
		/// <summary>
		/// 平均二消值
		/// </summary>
		public Nullable<int> avg_second_consumption{ get; set; }
		/// <summary>
		/// A 类用户数
		/// </summary>
		public Nullable<int> user_count_type_a{ get; set; }
		/// <summary>
		/// B 类用户数	
		/// </summary>
		public Nullable<int> user_count_type_b{ get; set; }
		/// <summary>
		/// C 类用户数
		/// </summary>
		public Nullable<int> user_count_type_c{ get; set; }
		/// <summary>
		/// 近 5 天 A 类用户数
		/// </summary>
		public Nullable<int> user_count_type_a_last_5d{ get; set; }
		/// <summary>
		/// 近 10 天 A 类用户数
		/// </summary>
		public Nullable<int> user_count_type_a_last_10d{ get; set; }
		/// <summary>
		/// 近 15 天 A 类用户数
		/// </summary>
		public Nullable<int> user_count_type_a_last_15d{ get; set; }
		/// <summary>
		/// 3 天内老用户数
		/// </summary>
		public Nullable<int> old_user_count_last_3d{ get; set; }
		/// <summary>
		/// 5 天内老用户数
		/// </summary>
		public Nullable<int> old_user_count_last_5d{ get; set; }
		/// <summary>
		/// 7 天内老用户数
		/// </summary>
		public Nullable<int> old_user_count_last_7d{ get; set; }
		/// <summary>
		/// 15天内老用户数
		/// </summary>
		public Nullable<int> old_user_count_last_15d{ get; set; }
		/// <summary>
		/// 5 天内升 A 类的用户数
		/// </summary>
		public Nullable<int> upgraded_to_a_count_last_5d{ get; set; }
		/// <summary>
		/// 总音浪
		/// </summary>
		public Nullable<int> totalFanTickets{ get; set; }
			}
	    			/// <summary>
			/// 表实体-数据比较规则 
			/// </summary>	
			public class dataanalysis_coredata_ting_rule : ModelDbBase
			{    
							public dataanalysis_coredata_ting_rule(){}
				public dataanalysis_coredata_ting_rule(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 规则：字段明:比较范围，比较直，颜色
		/// </summary>
		public string c_rule{ get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string remake{ get; set; }
			}
	    			/// <summary>
			/// 表实体-抖音数据-厅-每日核心-实时 
			/// </summary>	
			public class dataanalysis_ting_day_now : ModelDbBase
			{    
							public dataanalysis_ting_day_now(){}
				public dataanalysis_ting_day_now(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.target =  1;
														this.income =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属中台sn
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 目标音浪
		/// </summary>
		public Nullable<int> target{ get; set; }
		/// <summary>
		/// 当日音浪
		/// </summary>
		public Nullable<int> income{ get; set; }
			}
	    			/// <summary>
			/// 表实体-抖音数据-排档-厅 
			/// </summary>	
			public class doudata_dang_ting : ModelDbBase
			{    
							public doudata_dang_ting(){}
				public doudata_dang_ting(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.guestUcnt =  1;
														this.roomPCU =  1;
														this.giftUcnt =  1;
														this.voiceIncome =  1;
														this.anchorIncome =  1;
														this.unionGuestIncome =  1;
														this.guestIncome =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// anchorID
		/// </summary>
		public string dou_UID{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 场次id
		/// </summary>
		public string room_id{ get; set; }
		/// <summary>
		/// 开始时间
		/// </summary>
		public Nullable<DateTime> startTime{ get; set; }
		/// <summary>
		/// 结束时间
		/// </summary>
		public Nullable<DateTime> endTime{ get; set; }
		/// <summary>
		/// 嘉宾人数
		/// </summary>
		public Nullable<int> guestUcnt{ get; set; }
		/// <summary>
		/// 峰值人数
		/// </summary>
		public Nullable<int> roomPCU{ get; set; }
		/// <summary>
		/// 送礼人数
		/// </summary>
		public Nullable<int> giftUcnt{ get; set; }
		/// <summary>
		/// 语音聊天室音浪
		/// </summary>
		public Nullable<int> voiceIncome{ get; set; }
		/// <summary>
		/// 开播主播音浪
		/// </summary>
		public Nullable<int> anchorIncome{ get; set; }
		/// <summary>
		/// 连线嘉宾音浪（本公会）
		/// </summary>
		public Nullable<int> unionGuestIncome{ get; set; }
		/// <summary>
		/// 连线嘉宾音浪（非本公会）
		/// </summary>
		public Nullable<int> guestIncome{ get; set; }
			}
	    			/// <summary>
			/// 表实体-抖音数据-排档-主播 
			/// </summary>	
			public class doudata_dang_zhubo : ModelDbBase
			{    
							public doudata_dang_zhubo(){}
				public doudata_dang_zhubo(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.dang_id =  1;
														this.linkDuration =  1;
														this.linkIncome =  1;
														this.incomePrehour =  1;
														this.giftCnt =  1;
														this.giftUcnt =  1;
														this.amount =  1;
														this.new_num =  1;
														this.contact_num =  1;
														this.datou_num =  1;
														this.amount_1 =  1;
														this.num_2 =  1;
														this.amount_2 =  1;
														this.old_amount =  1;
														this.hx_num =  1;
														this.hx_amount =  1;
														this.hdpk_amount =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属主播sn
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 直播厅id
		/// </summary>
		public string room_id{ get; set; }
		/// <summary>
		/// 排档id
		/// </summary>
		public Nullable<int> dang_id{ get; set; }
		/// <summary>
		/// 连线时长(秒)
		/// </summary>
		public Nullable<int> linkDuration{ get; set; }
		/// <summary>
		/// 连线音浪
		/// </summary>
		public Nullable<int> linkIncome{ get; set; }
		/// <summary>
		/// 单小时连线音浪
		/// </summary>
		public Nullable<int> incomePrehour{ get; set; }
		/// <summary>
		/// 被打赏数
		/// </summary>
		public Nullable<int> giftCnt{ get; set; }
		/// <summary>
		/// 送礼人数
		/// </summary>
		public Nullable<int> giftUcnt{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<int> amount{ get; set; }
		/// <summary>
		/// 拉新数
		/// </summary>
		public Nullable<int> new_num{ get; set; }
		/// <summary>
		/// 建联数
		/// </summary>
		public Nullable<int> contact_num{ get; set; }
		/// <summary>
		/// 误刷大头（不小于99）
		/// </summary>
		public Nullable<int> datou_num{ get; set; }
		/// <summary>
		/// 首消音浪
		/// </summary>
		public Nullable<int> amount_1{ get; set; }
		/// <summary>
		/// 二消个数(用户个数)
		/// </summary>
		public Nullable<int> num_2{ get; set; }
		/// <summary>
		/// 二消音浪
		/// </summary>
		public Nullable<int> amount_2{ get; set; }
		/// <summary>
		/// 老用户音浪
		/// </summary>
		public Nullable<int> old_amount{ get; set; }
		/// <summary>
		/// 回消人数(2025-05-22新增)
		/// </summary>
		public Nullable<int> hx_num{ get; set; }
		/// <summary>
		/// 回消音浪(2025-05-22新增)
		/// </summary>
		public Nullable<int> hx_amount{ get; set; }
		/// <summary>
		/// 活动PK音浪(2025-05-22新增)
		/// </summary>
		public Nullable<int> hdpk_amount{ get; set; }
		/// <summary>
		/// 主播职务
		/// </summary>
		public string position{ get; set; }
			}
	    			/// <summary>
			/// 表实体-抖音数据-公会-每日核心（暂停） 
			/// </summary>	
			public class doudata_day_core : ModelDbBase
			{    
							public doudata_day_core(){}
				public doudata_day_core(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.income =  1;
														this.liveAnchorCnt =  1;
														this.vacationCnt =  1;
														this.newAnchor =  1;
														this.linkGuestCnt =  1;
														this.guestLiveIncome =  1;
														this.guestLiveUcnt =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 今日总音浪
		/// </summary>
		public Nullable<int> income{ get; set; }
		/// <summary>
		/// 今日开播人数
		/// </summary>
		public Nullable<int> liveAnchorCnt{ get; set; }
		/// <summary>
		/// 今日请假人数
		/// </summary>
		public Nullable<int> vacationCnt{ get; set; }
		/// <summary>
		/// 今日新入驻主播人数
		/// </summary>
		public Nullable<int> newAnchor{ get; set; }
		/// <summary>
		/// 今日连线嘉宾数
		/// </summary>
		public Nullable<int> linkGuestCnt{ get; set; }
		/// <summary>
		/// 今日嘉宾个播音浪
		/// </summary>
		public Nullable<int> guestLiveIncome{ get; set; }
		/// <summary>
		/// 今日嘉宾个播数
		/// </summary>
		public Nullable<int> guestLiveUcnt{ get; set; }
			}
	    			/// <summary>
			/// 表实体-抖音数据-公会-每日核心-小时（暂停） 
			/// </summary>	
			public class doudata_day_core_hour : ModelDbBase
			{    
							public doudata_day_core_hour(){}
				public doudata_day_core_hour(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.income =  1;
														this.liveAnchorCnt =  1;
														this.vacationCnt =  1;
														this.newAnchor =  1;
														this.linkGuestCnt =  1;
														this.guestLiveIncome =  1;
														this.guestLiveUcnt =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 小时
		/// </summary>
		public string hour{ get; set; }
		/// <summary>
		/// 今日总音浪
		/// </summary>
		public Nullable<int> income{ get; set; }
		/// <summary>
		/// 今日开播人数
		/// </summary>
		public Nullable<int> liveAnchorCnt{ get; set; }
		/// <summary>
		/// 今日请假人数
		/// </summary>
		public Nullable<int> vacationCnt{ get; set; }
		/// <summary>
		/// 今日新入驻主播人数
		/// </summary>
		public Nullable<int> newAnchor{ get; set; }
		/// <summary>
		/// 今日连线嘉宾数
		/// </summary>
		public Nullable<int> linkGuestCnt{ get; set; }
		/// <summary>
		/// 今日嘉宾个播音浪
		/// </summary>
		public Nullable<int> guestLiveIncome{ get; set; }
		/// <summary>
		/// 今日嘉宾个播数
		/// </summary>
		public Nullable<int> guestLiveUcnt{ get; set; }
			}
	    			/// <summary>
			/// 表实体-抖音数据-每日-厅（暂停） 
			/// </summary>	
			public class doudata_day_ting : ModelDbBase
			{    
							public doudata_day_ting(){}
				public doudata_day_ting(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.income =  1;
														this.datou =  1;
														this.daoju =  1;
														this.jiabing =  1;
														this.huodong =  1;
														this.huiyuan =  1;
														this.xing =  1;
														this.no_jiabing =  1;
														this.person_num =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// anchorID
		/// </summary>
		public string dou_UID{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 今日总音浪
		/// </summary>
		public Nullable<int> income{ get; set; }
		/// <summary>
		/// 误刷大头(礼物音浪)
		/// </summary>
		public Nullable<int> datou{ get; set; }
		/// <summary>
		/// 道具
		/// </summary>
		public Nullable<int> daoju{ get; set; }
		/// <summary>
		/// 嘉宾连线（嘉宾音浪）
		/// </summary>
		public Nullable<int> jiabing{ get; set; }
		/// <summary>
		/// 活动
		/// </summary>
		public Nullable<int> huodong{ get; set; }
		/// <summary>
		/// 会员订阅
		/// </summary>
		public Nullable<int> huiyuan{ get; set; }
		/// <summary>
		/// 星守护
		/// </summary>
		public Nullable<int> xing{ get; set; }
		/// <summary>
		/// 非入会嘉宾
		/// </summary>
		public Nullable<int> no_jiabing{ get; set; }
		/// <summary>
		/// 今日出勤人数
		/// </summary>
		public Nullable<int> person_num{ get; set; }
			}
	    			/// <summary>
			/// 表实体-抖音数据-日均-厅（暂停） 
			/// </summary>	
			public class doudata_day_ting_31 : ModelDbBase
			{    
							public doudata_day_ting_31(){}
				public doudata_day_ting_31(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.target_income =  1m;
														this.target_income_avg =  1m;
														this.income_1 =  1m;
														this.income_2 =  1m;
														this.income_3 =  1m;
														this.income_4 =  1m;
														this.income_5 =  1m;
														this.income_6 =  1m;
														this.income_7 =  1m;
														this.income_8 =  1m;
														this.income_9 =  1m;
														this.income_10 =  1m;
														this.tenday_income_1 =  1m;
														this.tenday_income_avg_1 =  1m;
														this.income_11 =  1m;
														this.income_12 =  1m;
														this.income_13 =  1m;
														this.income_14 =  1m;
														this.income_15 =  1m;
														this.income_16 =  1m;
														this.income_17 =  1m;
														this.income_18 =  1m;
														this.income_19 =  1m;
														this.income_20 =  1m;
														this.tenday_income_2 =  1m;
														this.tenday_income_avg_2 =  1m;
														this.income_21 =  1m;
														this.income_22 =  1m;
														this.income_23 =  1m;
														this.income_24 =  1m;
														this.income_25 =  1m;
														this.income_26 =  1m;
														this.income_27 =  1m;
														this.income_28 =  1m;
														this.income_29 =  1m;
														this.income_30 =  1m;
														this.income_31 =  1m;
														this.tenday_income_3 =  1m;
														this.tenday_income_avg_3 =  1m;
														this.total_income =  1m;
														this.total_income_avg =  1m;
														this.tenday_income_proportion_1 =  1m;
														this.undone_income_1 =  1m;
														this.tenday_income_proportion_2 =  1m;
														this.undone_income_2 =  1m;
														this.tenday_income_proportion_3 =  1m;
														this.undone_income_3 =  1m;
														this.tenday_income_proportion =  1m;
														this.undone_income =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 运营名
		/// </summary>
		public string yy_name{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 厅管名
		/// </summary>
		public string tg_name{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 厅名
		/// </summary>
		public string ting_name{ get; set; }
		/// <summary>
		/// 抖音号
		/// </summary>
		public string dou_user{ get; set; }
		/// <summary>
		/// 发生年月
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// target_amount=目标音浪；target_erxiao=目标二消；target_laxin=目标拉新；target_jianlian=目标建联;shiji_amount=实际音浪；shiji_erxiao=实际二消；shiji_laxin=实际拉新；shiji_jianlian=实际建联
		/// </summary>
		public string c_type{ get; set; }
		/// <summary>
		/// 月目标音浪
		/// </summary>
		public Nullable<decimal> target_income{ get; set; }
		/// <summary>
		/// 日均目标音浪
		/// </summary>
		public Nullable<decimal> target_income_avg{ get; set; }
		/// <summary>
		/// 值1
		/// </summary>
		public Nullable<decimal> income_1{ get; set; }
		/// <summary>
		/// 值2
		/// </summary>
		public Nullable<decimal> income_2{ get; set; }
		/// <summary>
		/// 值3
		/// </summary>
		public Nullable<decimal> income_3{ get; set; }
		/// <summary>
		/// 值4
		/// </summary>
		public Nullable<decimal> income_4{ get; set; }
		/// <summary>
		/// 值5
		/// </summary>
		public Nullable<decimal> income_5{ get; set; }
		/// <summary>
		/// 值6
		/// </summary>
		public Nullable<decimal> income_6{ get; set; }
		/// <summary>
		/// 值7
		/// </summary>
		public Nullable<decimal> income_7{ get; set; }
		/// <summary>
		/// 值8
		/// </summary>
		public Nullable<decimal> income_8{ get; set; }
		/// <summary>
		/// 值9
		/// </summary>
		public Nullable<decimal> income_9{ get; set; }
		/// <summary>
		/// 值10
		/// </summary>
		public Nullable<decimal> income_10{ get; set; }
		/// <summary>
		/// 十日值1
		/// </summary>
		public Nullable<decimal> tenday_income_1{ get; set; }
		/// <summary>
		/// 十日日均值1
		/// </summary>
		public Nullable<decimal> tenday_income_avg_1{ get; set; }
		/// <summary>
		/// 值11
		/// </summary>
		public Nullable<decimal> income_11{ get; set; }
		/// <summary>
		/// 值12
		/// </summary>
		public Nullable<decimal> income_12{ get; set; }
		/// <summary>
		/// 值13
		/// </summary>
		public Nullable<decimal> income_13{ get; set; }
		/// <summary>
		/// 值14
		/// </summary>
		public Nullable<decimal> income_14{ get; set; }
		/// <summary>
		/// 值15
		/// </summary>
		public Nullable<decimal> income_15{ get; set; }
		/// <summary>
		/// 值16
		/// </summary>
		public Nullable<decimal> income_16{ get; set; }
		/// <summary>
		/// 值17
		/// </summary>
		public Nullable<decimal> income_17{ get; set; }
		/// <summary>
		/// 值18
		/// </summary>
		public Nullable<decimal> income_18{ get; set; }
		/// <summary>
		/// 值19
		/// </summary>
		public Nullable<decimal> income_19{ get; set; }
		/// <summary>
		/// 值20
		/// </summary>
		public Nullable<decimal> income_20{ get; set; }
		/// <summary>
		/// 十日值2
		/// </summary>
		public Nullable<decimal> tenday_income_2{ get; set; }
		/// <summary>
		/// 十日日均值2
		/// </summary>
		public Nullable<decimal> tenday_income_avg_2{ get; set; }
		/// <summary>
		/// 值21
		/// </summary>
		public Nullable<decimal> income_21{ get; set; }
		/// <summary>
		/// 值22
		/// </summary>
		public Nullable<decimal> income_22{ get; set; }
		/// <summary>
		/// 值23
		/// </summary>
		public Nullable<decimal> income_23{ get; set; }
		/// <summary>
		/// 值24
		/// </summary>
		public Nullable<decimal> income_24{ get; set; }
		/// <summary>
		/// 值25
		/// </summary>
		public Nullable<decimal> income_25{ get; set; }
		/// <summary>
		/// 值26
		/// </summary>
		public Nullable<decimal> income_26{ get; set; }
		/// <summary>
		/// 值27
		/// </summary>
		public Nullable<decimal> income_27{ get; set; }
		/// <summary>
		/// 值28
		/// </summary>
		public Nullable<decimal> income_28{ get; set; }
		/// <summary>
		/// 值29
		/// </summary>
		public Nullable<decimal> income_29{ get; set; }
		/// <summary>
		/// 值30
		/// </summary>
		public Nullable<decimal> income_30{ get; set; }
		/// <summary>
		/// 值31
		/// </summary>
		public Nullable<decimal> income_31{ get; set; }
		/// <summary>
		/// 十日音浪3
		/// </summary>
		public Nullable<decimal> tenday_income_3{ get; set; }
		/// <summary>
		/// 十日日均音浪3
		/// </summary>
		public Nullable<decimal> tenday_income_avg_3{ get; set; }
		/// <summary>
		/// 总音浪
		/// </summary>
		public Nullable<decimal> total_income{ get; set; }
		/// <summary>
		/// 日均音浪
		/// </summary>
		public Nullable<decimal> total_income_avg{ get; set; }
		/// <summary>
		/// 第一阶段完成占比
		/// </summary>
		public Nullable<decimal> tenday_income_proportion_1{ get; set; }
		/// <summary>
		/// 第一阶段未完成音浪
		/// </summary>
		public Nullable<decimal> undone_income_1{ get; set; }
		/// <summary>
		/// 第二阶段完成占比
		/// </summary>
		public Nullable<decimal> tenday_income_proportion_2{ get; set; }
		/// <summary>
		/// 第二阶段未完成音浪
		/// </summary>
		public Nullable<decimal> undone_income_2{ get; set; }
		/// <summary>
		/// 第三阶段完成占比
		/// </summary>
		public Nullable<decimal> tenday_income_proportion_3{ get; set; }
		/// <summary>
		/// 第三阶段未完成音浪
		/// </summary>
		public Nullable<decimal> undone_income_3{ get; set; }
		/// <summary>
		/// 月度完成占比
		/// </summary>
		public Nullable<decimal> tenday_income_proportion{ get; set; }
		/// <summary>
		/// 月度未完成音浪
		/// </summary>
		public Nullable<decimal> undone_income{ get; set; }
			}
	    			/// <summary>
			/// 表实体-抖音数据-每日-主播 
			/// </summary>	
			public class doudata_day_zb : ModelDbBase
			{    
							public doudata_day_zb(){}
				public doudata_day_zb(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.income =  1;
														this.online =  1;
														this.gift_num =  1;
														this.new_income =  1;
														this.new_num =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属主播sn
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 当日总音浪
		/// </summary>
		public Nullable<int> income{ get; set; }
		/// <summary>
		/// 连线时长(分钟)
		/// </summary>
		public Nullable<int> online{ get; set; }
		/// <summary>
		/// 送礼次数
		/// </summary>
		public Nullable<int> gift_num{ get; set; }
		/// <summary>
		/// 新付费用户音浪
		/// </summary>
		public Nullable<int> new_income{ get; set; }
		/// <summary>
		/// 新付费用户人数
		/// </summary>
		public Nullable<int> new_num{ get; set; }
			}
	    			/// <summary>
			/// 表实体-抖音数据-场次-厅 
			/// </summary>	
			public class doudata_round_ting : ModelDbBase
			{    
							public doudata_round_ting(){}
				public doudata_round_ting(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.liveSeconds =  1;
														this.showUV =  1;
														this.showPV =  1;
														this.watchUV =  1;
														this.watchPV =  1;
														this.likePV =  1;
														this.commentUV =  1;
														this.payUV =  1;
														this.payPV =  1;
														this.avgWatchDuration =  1;
														this.liveFollowUV =  1;
														this.fanTicket =  1;
														this.acu =  1;
														this.live_rate =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// anchorID
		/// </summary>
		public string dou_UID{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 场次id
		/// </summary>
		public string room_id{ get; set; }
		/// <summary>
		/// 场次名称
		/// </summary>
		public string room_name{ get; set; }
		/// <summary>
		/// 直播开始时间
		/// </summary>
		public Nullable<DateTime> liveStartTime{ get; set; }
		/// <summary>
		/// 直播结束时间
		/// </summary>
		public Nullable<DateTime> liveEndTime{ get; set; }
		/// <summary>
		/// 直播时长（秒）
		/// </summary>
		public Nullable<int> liveSeconds{ get; set; }
		/// <summary>
		/// 曝光人数
		/// </summary>
		public Nullable<int> showUV{ get; set; }
		/// <summary>
		/// 曝光次数
		/// </summary>
		public Nullable<int> showPV{ get; set; }
		/// <summary>
		/// 进直播间人数
		/// </summary>
		public Nullable<int> watchUV{ get; set; }
		/// <summary>
		/// 进直播间次数
		/// </summary>
		public Nullable<int> watchPV{ get; set; }
		/// <summary>
		/// 点赞次数
		/// </summary>
		public Nullable<int> likePV{ get; set; }
		/// <summary>
		/// 评论人数
		/// </summary>
		public Nullable<int> commentUV{ get; set; }
		/// <summary>
		/// 送礼人数
		/// </summary>
		public Nullable<int> payUV{ get; set; }
		/// <summary>
		/// 送礼次数
		/// </summary>
		public Nullable<int> payPV{ get; set; }
		/// <summary>
		/// 人均观看时长（秒）
		/// </summary>
		public Nullable<int> avgWatchDuration{ get; set; }
		/// <summary>
		/// 新增粉丝
		/// </summary>
		public Nullable<int> liveFollowUV{ get; set; }
		/// <summary>
		/// 音浪
		/// </summary>
		public Nullable<int> fanTicket{ get; set; }
		/// <summary>
		/// ACU
		/// </summary>
		public Nullable<int> acu{ get; set; }
		/// <summary>
		/// 直播推荐比例
		/// </summary>
		public Nullable<decimal> live_rate{ get; set; }
			}
	    			/// <summary>
			/// 表实体-抖音数据-场次-厅-观众 
			/// </summary>	
			public class doudata_round_ting_guest : ModelDbBase
			{    
							public doudata_round_ting_guest(){}
				public doudata_round_ting_guest(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.likeGiftCnt =  1;
														this.watchDuration =  1;
														this.likeCnt =  1;
														this.commentCnt =  1;
														this.companyCnt =  1;
														this.honorLevelInt =  1;
														this.fanGroupLevelInt =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// anchorID
		/// </summary>
		public string dou_UID{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 场次id
		/// </summary>
		public string room_id{ get; set; }
		/// <summary>
		/// 观众ID
		/// </summary>
		public string userID{ get; set; }
		/// <summary>
		/// 观众名字
		/// </summary>
		public string nickname{ get; set; }
		/// <summary>
		/// 观众头像
		/// </summary>
		public string avatarUrl{ get; set; }
		/// <summary>
		/// 点赞送礼值
		/// </summary>
		public Nullable<int> likeGiftCnt{ get; set; }
		/// <summary>
		/// 观看时长
		/// </summary>
		public Nullable<int> watchDuration{ get; set; }
		/// <summary>
		/// 点赞数量
		/// </summary>
		public Nullable<int> likeCnt{ get; set; }
		/// <summary>
		/// 评论数量
		/// </summary>
		public Nullable<int> commentCnt{ get; set; }
		/// <summary>
		/// 近15天陪伴次数
		/// </summary>
		public Nullable<int> companyCnt{ get; set; }
		/// <summary>
		/// 用户荣誉等级
		/// </summary>
		public Nullable<int> honorLevelInt{ get; set; }
		/// <summary>
		/// 粉丝团等级
		/// </summary>
		public Nullable<int> fanGroupLevelInt{ get; set; }
		/// <summary>
		/// 新付费观众 0 = 否；1=是
		/// </summary>
		public Nullable<sbyte> is_new{ get; set; }
			}
	    			/// <summary>
			/// 表实体-抖音数据-厅-观众 
			/// </summary>	
			public class doudata_ting_guest : ModelDbBase
			{    
							public doudata_ting_guest(){}
				public doudata_ting_guest(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.likeGiftTotal =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// anchorID
		/// </summary>
		public string dou_UID{ get; set; }
		/// <summary>
		/// 观众ID
		/// </summary>
		public string userID{ get; set; }
		/// <summary>
		/// 观众名字
		/// </summary>
		public string nickname{ get; set; }
		/// <summary>
		/// 观众头像
		/// </summary>
		public string avatarUrl{ get; set; }
		/// <summary>
		/// 点赞送礼值(累计)
		/// </summary>
		public Nullable<int> likeGiftTotal{ get; set; }
		/// <summary>
		/// 点赞送礼值大于0小于等于99的日期（新付）
		/// </summary>
		public Nullable<DateTime> likeGift_date{ get; set; }
		/// <summary>
		/// 点赞送礼值达到99的日期（C类）
		/// </summary>
		public Nullable<DateTime> likeGift99_date{ get; set; }
		/// <summary>
		/// 点赞送礼值达到399的日期（B类）
		/// </summary>
		public Nullable<DateTime> likeGift399_date{ get; set; }
		/// <summary>
		/// 点赞送礼值达到999的日期（A类）
		/// </summary>
		public Nullable<DateTime> likeGift999_date{ get; set; }
			}
	    			/// <summary>
			/// 表实体-团队-运营团队 
			/// </summary>	
			public class group_yy : ModelDbBase
			{    
							public group_yy(){}
				public group_yy(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 团队名称
		/// </summary>
		public string group_name{ get; set; }
			}
	    			/// <summary>
			/// 表实体-帮助 
			/// </summary>	
			public class help : ModelDbBase
			{    
							public help(){}
				public help(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 帮助类别标识
		/// </summary>
		public string category_code{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 内容
		/// </summary>
		public string content{ get; set; }
			}
	    			/// <summary>
			/// 表实体-节奏-待删除 
			/// </summary>	
			public class jiezou : ModelDbBase
			{    
							public jiezou(){}
				public jiezou(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 轮次
		/// </summary>
		public string term{ get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		/// <summary>
		/// 所属运营
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 主表sn
		/// </summary>
		public string jiezou_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-节奏阶段-日 
			/// </summary>	
			public class jiezou_detail : ModelDbBase
			{    
							public jiezou_detail(){}
				public jiezou_detail(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.step =  1m;
														this.new_num_avg =  1m;
														this.zb_num =  1;
														this.session_count_avg =  1m;
														this.jll =  1m;
														this.exl =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 当前阶段;#enum:第一阶段=1;第二阶段=2;第三阶段=3;第四阶段=4;第五阶段=5
		/// </summary>
		public Nullable<decimal> step{ get; set; }
		public enum step_enum {
			第一阶段=1,
			第二阶段=2,
			第三阶段=3,
			第四阶段=4,
			第五阶段=5,
		}
		/// <summary>
		/// 数据时间
		/// </summary>
		public Nullable<DateTime> data_time{ get; set; }
		/// <summary>
		/// 厅下人均拉新
		/// </summary>
		public Nullable<decimal> new_num_avg{ get; set; }
		/// <summary>
		/// 人均拉新是否达标#enum: 否=0;是=1
		/// </summary>
		public Nullable<sbyte> is_new_num_avg{ get; set; }
		public enum is_new_num_avg_enum {
			 否=0,
			是=1,
		}
		/// <summary>
		/// 厅下主播提交数据的人数(不含请假与新人)
		/// </summary>
		public Nullable<int> zb_num{ get; set; }
		/// <summary>
		/// 厅下当日开档数量
		/// </summary>
		public Nullable<decimal> session_count_avg{ get; set; }
		/// <summary>
		/// 开档数量是否达标#enum: 否=0;是=1
		/// </summary>
		public Nullable<sbyte> is_session_count_avg{ get; set; }
		public enum is_session_count_avg_enum {
			 否=0,
			是=1,
		}
		/// <summary>
		/// 建联率
		/// </summary>
		public Nullable<decimal> jll{ get; set; }
		/// <summary>
		/// 建联率是否达标#enum: 否=0;是=1
		/// </summary>
		public Nullable<sbyte> is_jll{ get; set; }
		public enum is_jll_enum {
			 否=0,
			是=1,
		}
		/// <summary>
		/// 二消率
		/// </summary>
		public Nullable<decimal> exl{ get; set; }
		/// <summary>
		/// 二消率是否达标#enum: 否=0;是=1
		/// </summary>
		public Nullable<sbyte> is_exl{ get; set; }
		public enum is_exl_enum {
			 否=0,
			是=1,
		}
		/// <summary>
		/// 是否活动日#enum: 否=0;是=1
		/// </summary>
		public Nullable<sbyte> is_active{ get; set; }
		public enum is_active_enum {
			 否=0,
			是=1,
		}
		/// <summary>
		/// 按月度考核标准，当天是否达标#enum: 否=0;是=1
		/// </summary>
		public Nullable<sbyte> is_month_pass{ get; set; }
		public enum is_month_pass_enum {
			 否=0,
			是=1,
		}
		/// <summary>
		/// 月度考核过程明细备注
		/// </summary>
		public string month_pass_demo{ get; set; }
			}
	    			/// <summary>
			/// 表实体-节奏阶段-月度 
			/// </summary>	
			public class jiezou_detail_mouth : ModelDbBase
			{    
							public jiezou_detail_mouth(){}
				public jiezou_detail_mouth(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.new_num_avg =  1m;
														this.jll =  1m;
														this.exl =  1m;
														this.new_num_db_day =  1;
														this.exl_jll_db_day =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 期数sn
		/// </summary>
		public string jiezou_sn{ get; set; }
		/// <summary>
		/// 所属运营
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 运营名
		/// </summary>
		public string yy_name{ get; set; }
		/// <summary>
		/// 所属厅管
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 厅管名
		/// </summary>
		public string tg_name{ get; set; }
		/// <summary>
		/// 所属厅
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 厅名
		/// </summary>
		public string ting_name{ get; set; }
		/// <summary>
		/// 数据时间(表示当前半个月的数据时间)
		/// </summary>
		public Nullable<DateTime> data_time{ get; set; }
		/// <summary>
		/// 是否达标4.0阶段 1:达标 0:未达标
		/// </summary>
		public string is_standard{ get; set; }
		/// <summary>
		/// 厅下阶段人均拉新(废弃20250627)
		/// </summary>
		public Nullable<decimal> new_num_avg{ get; set; }
		/// <summary>
		/// 厅下阶段建联率(废弃20250627)
		/// </summary>
		public Nullable<decimal> jll{ get; set; }
		/// <summary>
		/// 厅下阶段二消率(废弃20250627)
		/// </summary>
		public Nullable<decimal> exl{ get; set; }
		/// <summary>
		/// 拉新达标1.5的天数
		/// </summary>
		public Nullable<int> new_num_db_day{ get; set; }
		/// <summary>
		/// 二消和建联达标的天数
		/// </summary>
		public Nullable<int> exl_jll_db_day{ get; set; }
			}
	    			/// <summary>
			/// 表实体-运营节奏-阶段-期数 
			/// </summary>	
			public class jiezou_detail_mouth_term : ModelDbBase
			{    
							public jiezou_detail_mouth_term(){}
				public jiezou_detail_mouth_term(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 期数sn
		/// </summary>
		public string jiezou_sn{ get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 阶段开始日期
		/// </summary>
		public Nullable<DateTime> s_date{ get; set; }
		/// <summary>
		/// 阶段结束日期
		/// </summary>
		public Nullable<DateTime> e_date{ get; set; }
			}
	    			/// <summary>
			/// 表实体-节奏规则表 
			/// </summary>	
			public class jiezou_detail_rule : ModelDbBase
			{    
							public jiezou_detail_rule(){}
				public jiezou_detail_rule(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.new_num_rule =  1m;
														this.num_2_rule =  1m;
														this.contact_num_rule =  1m;
														this.dangwei_rule =  1;
														this.amount_rule =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 规则名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 规则键
		/// </summary>
		public string key{ get; set; }
		/// <summary>
		/// 人均拉新达标规则
		/// </summary>
		public Nullable<decimal> new_num_rule{ get; set; }
		/// <summary>
		/// 二消率达标规则
		/// </summary>
		public Nullable<decimal> num_2_rule{ get; set; }
		/// <summary>
		/// 建联率达标规则
		/// </summary>
		public Nullable<decimal> contact_num_rule{ get; set; }
		/// <summary>
		/// 档位数达标规则
		/// </summary>
		public Nullable<int> dangwei_rule{ get; set; }
		/// <summary>
		/// 音浪数达标规则
		/// </summary>
		public Nullable<decimal> amount_rule{ get; set; }
		/// <summary>
		/// 规则类型#enum:日节奏统计规则=1;月节奏统计规则=2
		/// </summary>
		public string type{ get; set; }
		public enum type_enum {
			日节奏统计规则=1,
			月节奏统计规则=2,
		}
			}
	    			/// <summary>
			/// 表实体-节奏阶段-活动日 
			/// </summary>	
			public class jiezou_huodongri : ModelDbBase
			{    
							public jiezou_huodongri(){}
				public jiezou_huodongri(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 活动日
		/// </summary>
		public Nullable<DateTime> hd_date{ get; set; }
			}
	    			/// <summary>
			/// 表实体-待删除 
			/// </summary>	
			public class jiezou_item : ModelDbBase
			{    
							public jiezou_item(){}
				public jiezou_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.step =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 阶段1推人状态#enum:进行中=1;已完成=2
		/// </summary>
		public Nullable<sbyte> progress_1{ get; set; }
		public enum progress_1_enum {
			进行中=1,
			已完成=2,
		}
		/// <summary>
		/// 阶段1预计时间
		/// </summary>
		public Nullable<DateTime> time_plan_1{ get; set; }
		/// <summary>
		/// 阶段1完成时间
		/// </summary>
		public Nullable<DateTime> time_finish_1{ get; set; }
		/// <summary>
		/// 阶段2推人状态#enum:进行中=1;已完成=2
		/// </summary>
		public Nullable<sbyte> progress_2{ get; set; }
		public enum progress_2_enum {
			进行中=1,
			已完成=2,
		}
		/// <summary>
		/// 阶段2预计时间
		/// </summary>
		public Nullable<DateTime> time_plan_2{ get; set; }
		/// <summary>
		/// 阶段2完成时间
		/// </summary>
		public Nullable<DateTime> time_finish_2{ get; set; }
		/// <summary>
		/// 阶段3推人状态#enum:进行中=1;已完成=2
		/// </summary>
		public Nullable<sbyte> progress_3{ get; set; }
		public enum progress_3_enum {
			进行中=1,
			已完成=2,
		}
		/// <summary>
		/// 阶段3预计时间
		/// </summary>
		public Nullable<DateTime> time_plan_3{ get; set; }
		/// <summary>
		/// 阶段3完成时间
		/// </summary>
		public Nullable<DateTime> time_finish_3{ get; set; }
		/// <summary>
		/// 阶段4推人状态#enum:进行中=1;已完成=2
		/// </summary>
		public Nullable<sbyte> progress_4{ get; set; }
		public enum progress_4_enum {
			进行中=1,
			已完成=2,
		}
		/// <summary>
		/// 阶段4预计时间
		/// </summary>
		public Nullable<DateTime> time_plan_4{ get; set; }
		/// <summary>
		/// 阶段4完成时间
		/// </summary>
		public Nullable<DateTime> time_finish_4{ get; set; }
		/// <summary>
		/// 阶段5推人状态#enum:进行中=1;已完成=2
		/// </summary>
		public Nullable<sbyte> progress_5{ get; set; }
		public enum progress_5_enum {
			进行中=1,
			已完成=2,
		}
		/// <summary>
		/// 阶段5预计时间
		/// </summary>
		public Nullable<DateTime> time_plan_5{ get; set; }
		/// <summary>
		/// 阶段5完成时间
		/// </summary>
		public Nullable<DateTime> time_finish_5{ get; set; }
		/// <summary>
		/// 开始时间
		/// </summary>
		public Nullable<DateTime> start_date{ get; set; }
		/// <summary>
		/// 结束时间
		/// </summary>
		public Nullable<DateTime> end_date{ get; set; }
		/// <summary>
		/// 主表sn
		/// </summary>
		public string jiezou_sn{ get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		/// <summary>
		/// 当前阶段#enum:第一阶段=1;第二阶段=2;第三阶段=3;第四阶段=4;第五阶段=5
		/// </summary>
		public Nullable<decimal> step{ get; set; }
		public enum step_enum {
			第一阶段=1,
			第二阶段=2,
			第三阶段=3,
			第四阶段=4,
			第五阶段=5,
		}
		/// <summary>
		/// 阶段6推人状态#enum:进行中=1;已完成=2
		/// </summary>
		public Nullable<sbyte> progress_6{ get; set; }
		public enum progress_6_enum {
			进行中=1,
			已完成=2,
		}
		/// <summary>
		/// 阶段6预计时间
		/// </summary>
		public Nullable<DateTime> time_plan_6{ get; set; }
		/// <summary>
		/// 阶段6完成时间
		/// </summary>
		public Nullable<DateTime> time_finish_6{ get; set; }
		/// <summary>
		/// 阶段7推人状态#enum:进行中=1;已完成=2
		/// </summary>
		public Nullable<sbyte> progress_7{ get; set; }
		public enum progress_7_enum {
			进行中=1,
			已完成=2,
		}
		/// <summary>
		/// 阶段7预计时间
		/// </summary>
		public Nullable<DateTime> time_plan_7{ get; set; }
		/// <summary>
		/// 阶段7完成时间
		/// </summary>
		public Nullable<DateTime> time_finish_7{ get; set; }
			}
	    			/// <summary>
			/// 表实体-节奏阶段-主播 
			/// </summary>	
			public class jiezou_zhubo_detail : ModelDbBase
			{    
							public jiezou_zhubo_detail(){}
				public jiezou_zhubo_detail(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.step =  1m;
														this.new_num_avg =  1m;
														this.contact_num =  1m;
														this.num_2 =  1m;
														this.amount_2_avg =  1m;
														this.hx_amount_avg =  1m;
														this.hdpk_amount =  1m;
														this.session_count =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 主播厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 主播sn
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 当前阶段;#enum:第一阶段=1;第二阶段=2;第三阶段=3;第四阶段=4;第五阶段=5
		/// </summary>
		public Nullable<decimal> step{ get; set; }
		public enum step_enum {
			第一阶段=1,
			第二阶段=2,
			第三阶段=3,
			第四阶段=4,
			第五阶段=5,
		}
		/// <summary>
		/// 数据时间
		/// </summary>
		public Nullable<DateTime> data_time{ get; set; }
		/// <summary>
		/// 主播当日平均每档拉新
		/// </summary>
		public Nullable<decimal> new_num_avg{ get; set; }
		/// <summary>
		/// 主播当日总建联
		/// </summary>
		public Nullable<decimal> contact_num{ get; set; }
		/// <summary>
		/// 主播当日总二消个数
		/// </summary>
		public Nullable<decimal> num_2{ get; set; }
		/// <summary>
		/// 主播当日平均每档二消音浪
		/// </summary>
		public Nullable<decimal> amount_2_avg{ get; set; }
		/// <summary>
		/// 主播当日平均每档回消音浪
		/// </summary>
		public Nullable<decimal> hx_amount_avg{ get; set; }
		/// <summary>
		/// 主播当日活动的PK总流水
		/// </summary>
		public Nullable<decimal> hdpk_amount{ get; set; }
		/// <summary>
		/// 主播当日直播场次
		/// </summary>
		public Nullable<decimal> session_count{ get; set; }
			}
	    			/// <summary>
			/// 表实体-节奏规则表_主播 
			/// </summary>	
			public class jiezou_zhubo_detail_rule : ModelDbBase
			{    
							public jiezou_zhubo_detail_rule(){}
				public jiezou_zhubo_detail_rule(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.new_num_avg_rule =  1m;
														this.contact_num_rule =  1m;
														this.num_2_rule =  1m;
														this.amount_2_avg_rule =  1m;
														this.hx_amount_avg_rule =  1m;
														this.hdpk_amount_rule =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 规则名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 规则键
		/// </summary>
		public string key{ get; set; }
		/// <summary>
		/// 平均每档拉新数规则
		/// </summary>
		public Nullable<decimal> new_num_avg_rule{ get; set; }
		/// <summary>
		/// 总建联数规则
		/// </summary>
		public Nullable<decimal> contact_num_rule{ get; set; }
		/// <summary>
		/// 总二消个数规则
		/// </summary>
		public Nullable<decimal> num_2_rule{ get; set; }
		/// <summary>
		/// 二消音浪值规则
		/// </summary>
		public Nullable<decimal> amount_2_avg_rule{ get; set; }
		/// <summary>
		/// 回消音量值规则
		/// </summary>
		public Nullable<decimal> hx_amount_avg_rule{ get; set; }
		/// <summary>
		/// 活动PK音浪值规则
		/// </summary>
		public Nullable<decimal> hdpk_amount_rule{ get; set; }
			}
	    			/// <summary>
			/// 表实体-补人-统计表 
			/// </summary>	
			public class need_count : ModelDbBase
			{    
							public need_count(){}
				public need_count(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.need_sum =  1;
														this.male_sum =  1;
														this.female_sum =  1;
														this.finish_sum =  1;
														this.male_finish_sum =  1;
														this.female_finish_sum =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 统计日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 提报总数
		/// </summary>
		public Nullable<int> need_sum{ get; set; }
		/// <summary>
		/// 男性数量
		/// </summary>
		public Nullable<int> male_sum{ get; set; }
		/// <summary>
		/// 女性数量
		/// </summary>
		public Nullable<int> female_sum{ get; set; }
		/// <summary>
		/// 已补总数
		/// </summary>
		public Nullable<int> finish_sum{ get; set; }
		/// <summary>
		/// 男性已补数量
		/// </summary>
		public Nullable<int> male_finish_sum{ get; set; }
		/// <summary>
		/// 女性已补数量
		/// </summary>
		public Nullable<int> female_finish_sum{ get; set; }
			}
	    			/// <summary>
			/// 表实体-成才 
			/// </summary>	
			public class p_cencai : ModelDbBase
			{    
							public p_cencai(){}
				public p_cencai(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.round =  1;
														this.days =  1;
														this.target_lx =  1;
														this.target_erx =  1;
														this.target_jl =  1;
														this.target_yl =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 轮次
		/// </summary>
		public Nullable<int> round{ get; set; }
		/// <summary>
		/// 该轮次是否已结束#enum:未结束=1;已结束=2
		/// </summary>
		public byte? is_finished{ get; set; }
		public enum is_finished_enum {
			未结束=1,
			已结束=2,
		}
		/// <summary>
		/// 厅管sn
		/// </summary>
		public string tger_sn{ get; set; }
		/// <summary>
		/// 主播sn
		/// </summary>
		public string zber_sn{ get; set; }
		/// <summary>
		/// 统计开始日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 师父名字
		/// </summary>
		public string teacher{ get; set; }
		/// <summary>
		/// 考核天数
		/// </summary>
		public Nullable<int> days{ get; set; }
		/// <summary>
		/// 拉新目标
		/// </summary>
		public Nullable<int> target_lx{ get; set; }
		/// <summary>
		/// 二消目标
		/// </summary>
		public Nullable<int> target_erx{ get; set; }
		/// <summary>
		/// 建联目标
		/// </summary>
		public Nullable<int> target_jl{ get; set; }
		/// <summary>
		/// 流水目标（音浪）
		/// </summary>
		public Nullable<int> target_yl{ get; set; }
		/// <summary>
		/// 入职日期
		/// </summary>
		public Nullable<DateTime> join_date{ get; set; }
		/// <summary>
		/// MBTI
		/// </summary>
		public string MBTI{ get; set; }
		/// <summary>
		/// 学历
		/// </summary>
		public string education{ get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public Nullable<sbyte> zb_status{ get; set; }
			}
	    			/// <summary>
			/// 表实体-成才-PK 
			/// </summary>	
			public class p_cencai_pk : ModelDbBase
			{    
							public p_cencai_pk(){}
				public p_cencai_pk(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属pk_sn
		/// </summary>
		public string pk_sn{ get; set; }
		/// <summary>
		/// 创建人类型#enum:运营=1;超管=2
		/// </summary>
		public Nullable<sbyte> u_type{ get; set; }
		public enum u_type_enum {
			运营=1,
			超管=2,
		}
		/// <summary>
		/// 创建人user_sn
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// pk名称，如：新人组pk
		/// </summary>
		public string name{ get; set; }
			}
	    			/// <summary>
			/// 表实体-成才-PK-主播明细 
			/// </summary>	
			public class p_cencai_pk_item : ModelDbBase
			{    
							public p_cencai_pk_item(){}
				public p_cencai_pk_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属pk_sn
		/// </summary>
		public string pk_sn{ get; set; }
		/// <summary>
		/// 运营sn
		/// </summary>
		public string yyer_sn{ get; set; }
		/// <summary>
		/// 厅管sn
		/// </summary>
		public string tger_sn{ get; set; }
		/// <summary>
		/// 主播sn
		/// </summary>
		public string zber_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-主播成才-显示字段配置 
			/// </summary>	
			public class p_cencai_setting : ModelDbBase
			{    
							public p_cencai_setting(){}
				public p_cencai_setting(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属用户
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 显示师父名字#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> teacher{ get; set; }
		public enum teacher_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示主播名字#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> zber_name{ get; set; }
		public enum zber_name_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示角色#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> role{ get; set; }
		public enum role_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示年龄#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> age{ get; set; }
		public enum age_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示轮次#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> round{ get; set; }
		public enum round_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示考核天数#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> days{ get; set; }
		public enum days_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示总直播天数#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> live_days_total{ get; set; }
		public enum live_days_total_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示请假天数#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> vacation_days{ get; set; }
		public enum vacation_days_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示实际直播天数#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> live_days{ get; set; }
		public enum live_days_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示拉新#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> new_num{ get; set; }
		public enum new_num_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示二消#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> amount_2{ get; set; }
		public enum amount_2_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示建联#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> contact_num{ get; set; }
		public enum contact_num_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示平均拉新#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> new_num_avg{ get; set; }
		public enum new_num_avg_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示二消率#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> amount_2_rate{ get; set; }
		public enum amount_2_rate_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示建联率#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> contact_num_rate{ get; set; }
		public enum contact_num_rate_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 显示时间进度#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> time_progress{ get; set; }
		public enum time_progress_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 拉新二消流水#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> new_2_amount{ get; set; }
		public enum new_2_amount_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 老用户流水#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> old_amount{ get; set; }
		public enum old_amount_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 活动流水#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> activity_amount{ get; set; }
		public enum activity_amount_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 流水进度#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> amount_progress{ get; set; }
		public enum amount_progress_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 流水累计#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> amount{ get; set; }
		public enum amount_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 入职日期#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> join_date{ get; set; }
		public enum join_date_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// MBTI#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> MBTI{ get; set; }
		public enum MBTI_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 学历#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> education{ get; set; }
		public enum education_enum {
			显示=1,
			不显示=0,
		}
		/// <summary>
		/// 状态#enum:显示=1;不显示=0
		/// </summary>
		public Nullable<sbyte> zb_status{ get; set; }
		public enum zb_status_enum {
			显示=1,
			不显示=0,
		}
			}
	    			/// <summary>
			/// 表实体-培训课程-已学课程 
			/// </summary>	
			public class p_course_studied : ModelDbBase
			{    
							public p_course_studied(){}
				public p_course_studied(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.course_id =  1;
														this.mintues =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 用户编号
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 课程id
		/// </summary>
		public Nullable<int> course_id{ get; set; }
		/// <summary>
		/// 学习时长
		/// </summary>
		public Nullable<int> mintues{ get; set; }
			}
	    			/// <summary>
			/// 表实体-培训课程-视频 
			/// </summary>	
			public class p_course_video : ModelDbBase
			{    
							public p_course_video(){}
				public p_course_video(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.mintues =  1;
														this.category_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 类型11=主播；10=厅管
		/// </summary>
		public Nullable<sbyte> v_type{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 视频封面图片地址
		/// </summary>
		public string pic_url{ get; set; }
		/// <summary>
		/// 视频文件地址
		/// </summary>
		public string video_url{ get; set; }
		/// <summary>
		/// 视频时长
		/// </summary>
		public Nullable<int> mintues{ get; set; }
		/// <summary>
		/// 分类id
		/// </summary>
		public Nullable<int> category_id{ get; set; }
		/// <summary>
		/// 排序
		/// </summary>
		public Nullable<int> sort{ get; set; }
		/// <summary>
		/// 底部-详细介绍
		/// </summary>
		public string introduce_bottom{ get; set; }
		/// <summary>
		/// 右侧-简要介绍
		/// </summary>
		public string introduce_right{ get; set; }
			}
	    			/// <summary>
			/// 表实体-课程-视频-类别 
			/// </summary>	
			public class p_course_video_category : ModelDbBase
			{    
							public p_course_video_category(){}
				public p_course_video_category(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.parent_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 类型#enum:主播=11;厅管=10;运营=12
		/// </summary>
		public Nullable<sbyte> v_type{ get; set; }
		public enum v_type_enum {
			主播=11,
			厅管=10,
			运营=12,
		}
		/// <summary>
		/// 父级id
		/// </summary>
		public Nullable<int> parent_id{ get; set; }
		/// <summary>
		/// 类别名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 排序，越小越靠前
		/// </summary>
		public Nullable<int> sort{ get; set; }
			}
	    			/// <summary>
			/// 表实体-crm_用户 
			/// </summary>	
			public class p_crm_customer : ModelTmplModularCrm.crm_tmpl
			{    
							public p_crm_customer(){}
				public p_crm_customer(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// crm唯一编号
		/// </summary>
		public string crm_sn{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string nick{ get; set; }
		/// <summary>
		/// 用户抖音号
		/// </summary>
		public string dou_user{ get; set; }
		/// <summary>
		/// 用户定级
		/// </summary>
		public string user_grade{ get; set; }
		/// <summary>
		/// 家乡/现居地
		/// </summary>
		public string address{ get; set; }
		/// <summary>
		/// 与用户的认识方式
		/// </summary>
		public string know_type{ get; set; }
		/// <summary>
		/// 用户职业
		/// </summary>
		public string user_job{ get; set; }
		/// <summary>
		/// 用户生活作息
		/// </summary>
		public string user_life{ get; set; }
		/// <summary>
		/// 感情状态
		/// </summary>
		public string emo_status{ get; set; }
		/// <summary>
		/// 爱好（听歌、游戏）
		/// </summary>
		public string user_like{ get; set; }
		/// <summary>
		/// 生日
		/// </summary>
		public string user_birthday{ get; set; }
		/// <summary>
		/// 第一次进厅时间
		/// </summary>
		public Nullable<DateTime> first_time{ get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string demo{ get; set; }
		/// <summary>
		/// 用户等级
		/// </summary>
		public string user_level{ get; set; }
		/// <summary>
		/// 人格属性
		/// </summary>
		public string mbti{ get; set; }
		/// <summary>
		/// 用户头像url
		/// </summary>
		public string img_url{ get; set; }
		/// <summary>
		/// 是否加V
		/// </summary>
		public string has_wechat{ get; set; }
		/// <summary>
		/// 首次消费值
		/// </summary>
		public string first_consum{ get; set; }
		/// <summary>
		/// 建联时间
		/// </summary>
		public Nullable<DateTime> contact_time{ get; set; }
		/// <summary>
		/// 状态#enum:正常=0;逻辑删除=9
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			正常=0,
			逻辑删除=9,
		}
			}
	    			/// <summary>
			/// 表实体-绩效-档表 
			/// </summary>	
			public class p_jixiao_dangbiao : ModelDbBase
			{    
							public p_jixiao_dangbiao(){}
				public p_jixiao_dangbiao(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 档表sn
		/// </summary>
		public string db_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 开始日期
		/// </summary>
		public Nullable<DateTime> s_date{ get; set; }
		/// <summary>
		/// 结束日期
		/// </summary>
		public Nullable<DateTime> e_date{ get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string note{ get; set; }
		/// <summary>
		/// 档位角色
		/// </summary>
		public string dang_wei_role{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-档表-明细 
			/// </summary>	
			public class p_jixiao_dangbiao_item : ModelDbBase
			{    
							public p_jixiao_dangbiao_item(){}
				public p_jixiao_dangbiao_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 档表sn
		/// </summary>
		public string db_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 主播sn(0-1点)
		/// </summary>
		public string zb_0{ get; set; }
		/// <summary>
		/// 主播sn(1-2点)
		/// </summary>
		public string zb_1{ get; set; }
		/// <summary>
		/// 主播sn(2-3点)
		/// </summary>
		public string zb_2{ get; set; }
		/// <summary>
		/// 主播sn(3-4点)
		/// </summary>
		public string zb_3{ get; set; }
		/// <summary>
		/// 主播sn(4-5点)
		/// </summary>
		public string zb_4{ get; set; }
		/// <summary>
		/// 主播sn(5-6点)
		/// </summary>
		public string zb_5{ get; set; }
		/// <summary>
		/// 主播sn(6-7点)
		/// </summary>
		public string zb_6{ get; set; }
		/// <summary>
		/// 主播sn(7-8点)
		/// </summary>
		public string zb_7{ get; set; }
		/// <summary>
		/// 主播sn(8-9点)
		/// </summary>
		public string zb_8{ get; set; }
		/// <summary>
		/// 主播sn(9-10点)
		/// </summary>
		public string zb_9{ get; set; }
		/// <summary>
		/// 主播sn(10-11点)
		/// </summary>
		public string zb_10{ get; set; }
		/// <summary>
		/// 主播sn(11-12点)
		/// </summary>
		public string zb_11{ get; set; }
		/// <summary>
		/// 主播sn(12-13点)
		/// </summary>
		public string zb_12{ get; set; }
		/// <summary>
		/// 主播sn(13-14点)
		/// </summary>
		public string zb_13{ get; set; }
		/// <summary>
		/// 主播sn(14-15点)
		/// </summary>
		public string zb_14{ get; set; }
		/// <summary>
		/// 主播sn(15-16点)
		/// </summary>
		public string zb_15{ get; set; }
		/// <summary>
		/// 主播sn(16-17点)
		/// </summary>
		public string zb_16{ get; set; }
		/// <summary>
		/// 主播sn(17-18点)
		/// </summary>
		public string zb_17{ get; set; }
		/// <summary>
		/// 主播sn(18-19点)
		/// </summary>
		public string zb_18{ get; set; }
		/// <summary>
		/// 主播sn(19-20点)
		/// </summary>
		public string zb_19{ get; set; }
		/// <summary>
		/// 主播sn(20-21点)
		/// </summary>
		public string zb_20{ get; set; }
		/// <summary>
		/// 主播sn(21-22点)
		/// </summary>
		public string zb_21{ get; set; }
		/// <summary>
		/// 主播sn(22-23点)
		/// </summary>
		public string zb_22{ get; set; }
		/// <summary>
		/// 主播sn(23-24点)
		/// </summary>
		public string zb_23{ get; set; }
		/// <summary>
		/// 备注信息，只有每档的第一条记录可以填写
		/// </summary>
		public string memo{ get; set; }
		/// <summary>
		/// 主播胜任麦 R:红色三麦，G:绿色一麦，O:橙色二麦
		/// </summary>
		public string zbsr_color_db{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-每日上报 
			/// </summary>	
			public class p_jixiao_day : ModelDbBase
			{    
							public p_jixiao_day(){}
				public p_jixiao_day(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.amount =  1;
														this.new_num =  1;
														this.contact_num =  1;
														this.datou_num =  1;
														this.amount_1 =  1;
														this.num_2 =  1;
														this.amount_2 =  1;
														this.old_amount =  1;
														this.hx_num =  1;
														this.hx_amount =  1;
														this.hdpk_amount =  1;
														this.session_count =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅编号
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 绩效发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<int> amount{ get; set; }
		/// <summary>
		/// 拉新数
		/// </summary>
		public Nullable<int> new_num{ get; set; }
		/// <summary>
		/// 建联数
		/// </summary>
		public Nullable<int> contact_num{ get; set; }
		/// <summary>
		/// 误刷大头（不小于99）
		/// </summary>
		public Nullable<int> datou_num{ get; set; }
		/// <summary>
		/// 首消音浪
		/// </summary>
		public Nullable<int> amount_1{ get; set; }
		/// <summary>
		/// 二消个数
		/// </summary>
		public Nullable<int> num_2{ get; set; }
		/// <summary>
		/// 二消音浪
		/// </summary>
		public Nullable<int> amount_2{ get; set; }
		/// <summary>
		/// 老用户
		/// </summary>
		public Nullable<int> old_amount{ get; set; }
		/// <summary>
		/// 回消人数(2025-05-22新增)
		/// </summary>
		public Nullable<int> hx_num{ get; set; }
		/// <summary>
		/// 回消音浪(2025-05-22新增)
		/// </summary>
		public Nullable<int> hx_amount{ get; set; }
		/// <summary>
		/// 活动PK音浪(2025-05-22新增)
		/// </summary>
		public Nullable<int> hdpk_amount{ get; set; }
		/// <summary>
		/// 兼职/全职
		/// </summary>
		public string job{ get; set; }
		/// <summary>
		/// 总直播场次次数
		/// </summary>
		public Nullable<int> session_count{ get; set; }
		/// <summary>
		/// 今日总结
		/// </summary>
		public string summary_demo{ get; set; }
		/// <summary>
		/// 今日问题
		/// </summary>
		public string question_demo{ get; set; }
		/// <summary>
		/// 反思
		/// </summary>
		public string review_demo{ get; set; }
		/// <summary>
		/// 是否新人#enum:否=0;是=1
		/// </summary>
		public Nullable<sbyte> is_newer{ get; set; }
		public enum is_newer_enum {
			否=0,
			是=1,
		}
			}
	    			/// <summary>
			/// 表实体-绩效-每日上报 
			/// </summary>	
			public class p_jixiao_day_his : ModelDbBase
			{    
							public p_jixiao_day_his(){}
				public p_jixiao_day_his(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.amount =  1;
														this.new_num =  1;
														this.contact_num =  1;
														this.datou_num =  1;
														this.amount_1 =  1;
														this.num_2 =  1;
														this.amount_2 =  1;
														this.old_amount =  1;
														this.hx_num =  1;
														this.hx_amount =  1;
														this.hdpk_amount =  1;
														this.session_count =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅编号
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 绩效发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<int> amount{ get; set; }
		/// <summary>
		/// 拉新数
		/// </summary>
		public Nullable<int> new_num{ get; set; }
		/// <summary>
		/// 建联数
		/// </summary>
		public Nullable<int> contact_num{ get; set; }
		/// <summary>
		/// 误刷大头（不小于99）
		/// </summary>
		public Nullable<int> datou_num{ get; set; }
		/// <summary>
		/// 首消音浪
		/// </summary>
		public Nullable<int> amount_1{ get; set; }
		/// <summary>
		/// 二消个数
		/// </summary>
		public Nullable<int> num_2{ get; set; }
		/// <summary>
		/// 二消音浪
		/// </summary>
		public Nullable<int> amount_2{ get; set; }
		/// <summary>
		/// 老用户
		/// </summary>
		public Nullable<int> old_amount{ get; set; }
		/// <summary>
		/// 回消人数(2025-05-22新增)
		/// </summary>
		public Nullable<int> hx_num{ get; set; }
		/// <summary>
		/// 回消音浪(2025-05-22新增)
		/// </summary>
		public Nullable<int> hx_amount{ get; set; }
		/// <summary>
		/// 活动PK音浪(2025-05-22新增)
		/// </summary>
		public Nullable<int> hdpk_amount{ get; set; }
		/// <summary>
		/// 兼职/全职
		/// </summary>
		public string job{ get; set; }
		/// <summary>
		/// 总直播场次次数
		/// </summary>
		public Nullable<int> session_count{ get; set; }
		/// <summary>
		/// 今日总结
		/// </summary>
		public string summary_demo{ get; set; }
		/// <summary>
		/// 今日问题
		/// </summary>
		public string question_demo{ get; set; }
		/// <summary>
		/// 反思
		/// </summary>
		public string review_demo{ get; set; }
		/// <summary>
		/// 是否新人#enum:否=0;是=1
		/// </summary>
		public Nullable<sbyte> is_newer{ get; set; }
		public enum is_newer_enum {
			否=0,
			是=1,
		}
			}
	    			/// <summary>
			/// 表实体-绩效-每日上报-主播场次数据 
			/// </summary>	
			public class p_jixiao_day_session : ModelDbBase
			{    
							public p_jixiao_day_session(){}
				public p_jixiao_day_session(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.session =  1;
														this.amount =  1;
														this.new_num =  1;
														this.contact_num =  1;
														this.datou_num =  1;
														this.amount_1 =  1;
														this.num_2 =  1;
														this.amount_2 =  1;
														this.old_amount =  1;
														this.hx_num =  1;
														this.hx_amount =  1;
														this.hdpk_amount =  1;
														this.hanhuo_num1 =  1;
														this.new_pay_num1 =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 提交人所属用户编号
		/// </summary>
		public string op_user_sn{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 绩效发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 场次
		/// </summary>
		public Nullable<int> session{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<int> amount{ get; set; }
		/// <summary>
		/// 拉新数
		/// </summary>
		public Nullable<int> new_num{ get; set; }
		/// <summary>
		/// 建联数
		/// </summary>
		public Nullable<int> contact_num{ get; set; }
		/// <summary>
		/// 误刷大头（不小于99）
		/// </summary>
		public Nullable<int> datou_num{ get; set; }
		/// <summary>
		/// 首消音浪
		/// </summary>
		public Nullable<int> amount_1{ get; set; }
		/// <summary>
		/// 二消个数
		/// </summary>
		public Nullable<int> num_2{ get; set; }
		/// <summary>
		/// 二消音浪
		/// </summary>
		public Nullable<int> amount_2{ get; set; }
		/// <summary>
		/// 老用户
		/// </summary>
		public Nullable<int> old_amount{ get; set; }
		/// <summary>
		/// 回消人数(2025-05-22新增)
		/// </summary>
		public Nullable<int> hx_num{ get; set; }
		/// <summary>
		/// 回消音浪(2025-05-22新增)
		/// </summary>
		public Nullable<int> hx_amount{ get; set; }
		/// <summary>
		/// 活动PK音浪(2025-05-22新增)
		/// </summary>
		public Nullable<int> hdpk_amount{ get; set; }
		/// <summary>
		/// 是否为组长
		/// </summary>
		public Nullable<sbyte> is_leader{ get; set; }
		/// <summary>
		/// 是否请假
		/// </summary>
		public Nullable<sbyte> is_rest{ get; set; }
		/// <summary>
		/// 是否挂麦
		/// </summary>
		public Nullable<sbyte> is_guamai{ get; set; }
		/// <summary>
		/// 喊活人数
		/// </summary>
		public Nullable<int> hanhuo_num1{ get; set; }
		/// <summary>
		/// 新付费人数
		/// </summary>
		public Nullable<int> new_pay_num1{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-每日上报-主播场次数据 
			/// </summary>	
			public class p_jixiao_day_session_his : ModelDbBase
			{    
							public p_jixiao_day_session_his(){}
				public p_jixiao_day_session_his(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.session =  1;
														this.amount =  1;
														this.new_num =  1;
														this.contact_num =  1;
														this.datou_num =  1;
														this.amount_1 =  1;
														this.num_2 =  1;
														this.amount_2 =  1;
														this.old_amount =  1;
														this.hx_num =  1;
														this.hx_amount =  1;
														this.hdpk_amount =  1;
														this.hanhuo_num1 =  1;
														this.new_pay_num1 =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 绩效发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 场次
		/// </summary>
		public Nullable<int> session{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<int> amount{ get; set; }
		/// <summary>
		/// 拉新数
		/// </summary>
		public Nullable<int> new_num{ get; set; }
		/// <summary>
		/// 建联数
		/// </summary>
		public Nullable<int> contact_num{ get; set; }
		/// <summary>
		/// 误刷大头（不小于99）
		/// </summary>
		public Nullable<int> datou_num{ get; set; }
		/// <summary>
		/// 首消音浪
		/// </summary>
		public Nullable<int> amount_1{ get; set; }
		/// <summary>
		/// 二消个数
		/// </summary>
		public Nullable<int> num_2{ get; set; }
		/// <summary>
		/// 二消音浪
		/// </summary>
		public Nullable<int> amount_2{ get; set; }
		/// <summary>
		/// 老用户
		/// </summary>
		public Nullable<int> old_amount{ get; set; }
		/// <summary>
		/// 回消人数(2025-05-22新增)
		/// </summary>
		public Nullable<int> hx_num{ get; set; }
		/// <summary>
		/// 回消音浪(2025-05-22新增)
		/// </summary>
		public Nullable<int> hx_amount{ get; set; }
		/// <summary>
		/// 活动PK音浪(2025-05-22新增)
		/// </summary>
		public Nullable<int> hdpk_amount{ get; set; }
		/// <summary>
		/// 是否为组长
		/// </summary>
		public Nullable<sbyte> is_leader{ get; set; }
		/// <summary>
		/// 是否请假
		/// </summary>
		public Nullable<sbyte> is_rest{ get; set; }
		/// <summary>
		/// 是否挂麦
		/// </summary>
		public Nullable<sbyte> is_guamai{ get; set; }
		/// <summary>
		/// 喊活人数
		/// </summary>
		public Nullable<int> hanhuo_num1{ get; set; }
		/// <summary>
		/// 新付费人数
		/// </summary>
		public Nullable<int> new_pay_num1{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-每日上报-厅场次数据 
			/// </summary>	
			public class p_jixiao_day_session_total : ModelDbBase
			{    
							public p_jixiao_day_session_total(){}
				public p_jixiao_day_session_total(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.session =  1;
														this.hanhuo_num =  1;
														this.xinfu_num =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 绩效发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 场次
		/// </summary>
		public Nullable<int> session{ get; set; }
		/// <summary>
		/// 喊活人数
		/// </summary>
		public Nullable<int> hanhuo_num{ get; set; }
		/// <summary>
		/// 新付人数
		/// </summary>
		public Nullable<int> xinfu_num{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-每天-厅收益上交 
			/// </summary>	
			public class p_jixiao_day_ting_shangjiao : ModelDbBase
			{    
							public p_jixiao_day_ting_shangjiao(){}
				public p_jixiao_day_ting_shangjiao(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.income =  1m;
														this.datou =  1m;
														this.xing =  1m;
														this.huodong =  1m;
														this.amount =  1m;
														this.amounted =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅编号
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 收益发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 连线收入服务费
		/// </summary>
		public Nullable<decimal> income{ get; set; }
		/// <summary>
		/// 误刷大头(礼物音浪)
		/// </summary>
		public Nullable<decimal> datou{ get; set; }
		/// <summary>
		/// 星守护
		/// </summary>
		public Nullable<decimal> xing{ get; set; }
		/// <summary>
		/// 活动
		/// </summary>
		public Nullable<decimal> huodong{ get; set; }
		/// <summary>
		/// 应付金额
		/// </summary>
		public Nullable<decimal> amount{ get; set; }
		/// <summary>
		/// 已付金额
		/// </summary>
		public Nullable<decimal> amounted{ get; set; }
		/// <summary>
		/// 状态#enum:未上交=0;已上交=1;待生成=-1
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			未上交=0,
			已上交=1,
			待生成=-1,
		}
			}
	    			/// <summary>
			/// 表实体-绩效-每日上报-档类型(活动设置) 
			/// </summary>	
			public class p_jixiao_day_type : ModelDbBase
			{    
							public p_jixiao_day_type(){}
				public p_jixiao_day_type(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.session =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 绩效发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 场次
		/// </summary>
		public Nullable<int> session{ get; set; }
		/// <summary>
		/// 类型#enum:游戏档=1
		/// </summary>
		public Nullable<sbyte> c_type{ get; set; }
		public enum c_type_enum {
			游戏档=1,
		}
		/// <summary>
		/// 选项备注
		/// </summary>
		public string note{ get; set; }
		/// <summary>
		/// 直播厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 自定义直播类型文本
		/// </summary>
		public string type_name{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-厅战数据 
			/// </summary>	
			public class p_jixiao_kuafang : ModelDbBase
			{    
							public p_jixiao_kuafang(){}
				public p_jixiao_kuafang(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.old_amount =  1m;
														this.amount_2030 =  1m;
														this.amount_2040 =  1m;
														this.amount_2050 =  1m;
														this.amount_2100 =  1m;
														this.amount_2110 =  1m;
														this.amount_2120 =  1m;
														this.amount_2130 =  1m;
														this.amount_2140 =  1m;
														this.amount_2150 =  1m;
														this.amount_2200 =  1m;
														this.amount_2210 =  1m;
														this.amount_2220 =  1m;
														this.amount_2230 =  1m;
														this.amount_2240 =  1m;
														this.amount_2250 =  1m;
														this.amount_2300 =  1m;
														this.amount_2310 =  1m;
														this.amount_2320 =  1m;
														this.amount_2330 =  1m;
														this.amount_2340 =  1m;
														this.amount_2350 =  1m;
														this.amount_2400 =  1m;
														this.amount_total =  1m;
														this.amount_total_day =  1m;
														this.ting_num =  1;
														this.user_type_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 绩效发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 跨房前流水
		/// </summary>
		public Nullable<decimal> old_amount{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2030{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2040{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2050{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2100{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2110{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2120{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2130{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2140{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2150{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2200{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2210{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2220{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2230{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2240{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2250{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2300{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2310{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2320{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2330{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2340{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2350{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2400{ get; set; }
		/// <summary>
		/// 跨房总流水
		/// </summary>
		public Nullable<decimal> amount_total{ get; set; }
		/// <summary>
		/// 跨房当天总流水
		/// </summary>
		public Nullable<decimal> amount_total_day{ get; set; }
		/// <summary>
		/// 参加跨房厅数
		/// </summary>
		public Nullable<int> ting_num{ get; set; }
		/// <summary>
		/// 提交人user_sn
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 用户类型id
		/// </summary>
		public Nullable<int> user_type_id{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-请假 
			/// </summary>	
			public class p_jixiao_qingjia : ModelDbBase
			{    
							public p_jixiao_qingjia(){}
				public p_jixiao_qingjia(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属中台用户编号
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 替档主播用户编号
		/// </summary>
		public string new_zb_user_sn{ get; set; }
		/// <summary>
		/// 开始日期
		/// </summary>
		public Nullable<DateTime> s_date{ get; set; }
		/// <summary>
		/// 结束日期
		/// </summary>
		public Nullable<DateTime> e_date{ get; set; }
		/// <summary>
		/// 事由说明
		/// </summary>
		public string note{ get; set; }
		/// <summary>
		/// 状态#enum:等待审批=0;审批同意=1;审批拒绝=2;已取消=9
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			等待审批=0,
			审批同意=1,
			审批拒绝=2,
			已取消=9,
		}
			}
	    			/// <summary>
			/// 表实体-绩效-业绩标准 
			/// </summary>	
			public class p_jixiao_standard : ModelDbBase
			{    
							public p_jixiao_standard(){}
				public p_jixiao_standard(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.amount =  1m;
														this.new_num =  1;
														this.amount_2_rate =  1m;
														this.amount_2 =  1m;
														this.contact_num =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 统计日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 开始日期
		/// </summary>
		public Nullable<DateTime> s_date{ get; set; }
		/// <summary>
		/// 结束日期
		/// </summary>
		public Nullable<DateTime> e_date{ get; set; }
		/// <summary>
		/// 主表编号
		/// </summary>
		public string st_sn{ get; set; }
		/// <summary>
		/// 音浪考核标准
		/// </summary>
		public Nullable<decimal> amount{ get; set; }
		/// <summary>
		/// 拉新考核标准
		/// </summary>
		public Nullable<int> new_num{ get; set; }
		/// <summary>
		/// 二消率考核标准
		/// </summary>
		public Nullable<decimal> amount_2_rate{ get; set; }
		/// <summary>
		/// 二消数指标
		/// </summary>
		public Nullable<decimal> amount_2{ get; set; }
		/// <summary>
		/// 建联数指标
		/// </summary>
		public Nullable<decimal> contact_num{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-业绩标准 
			/// </summary>	
			public class p_jixiao_standard_item : ModelDbBase
			{    
							public p_jixiao_standard_item(){}
				public p_jixiao_standard_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.amount =  1m;
														this.new_num =  1;
														this.amount_2_rate =  1m;
														this.amount_2 =  1m;
														this.contact_num =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 音浪考核标准
		/// </summary>
		public Nullable<decimal> amount{ get; set; }
		/// <summary>
		/// 拉新考核标准
		/// </summary>
		public Nullable<int> new_num{ get; set; }
		/// <summary>
		/// 二消率考核标准
		/// </summary>
		public Nullable<decimal> amount_2_rate{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 主表sn
		/// </summary>
		public string st_sn{ get; set; }
		/// <summary>
		/// 二消数指标
		/// </summary>
		public Nullable<decimal> amount_2{ get; set; }
		/// <summary>
		/// 建联数指标
		/// </summary>
		public Nullable<decimal> contact_num{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-厅目标 
			/// </summary>	
			public class p_jixiao_target_tg : ModelDbBase
			{    
							public p_jixiao_target_tg(){}
				public p_jixiao_target_tg(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.amount =  1m;
														this.amount_target_1 =  1m;
														this.amount_target_2 =  1m;
														this.amount_target_3 =  1m;
														this.new_num =  1;
														this.amount_2 =  1m;
														this.num_2 =  1;
														this.contact_num =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 目标月份
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 目标音浪
		/// </summary>
		public Nullable<decimal> amount{ get; set; }
		/// <summary>
		/// 目标音浪-阶段一
		/// </summary>
		public Nullable<decimal> amount_target_1{ get; set; }
		/// <summary>
		/// 目标音浪-阶段二
		/// </summary>
		public Nullable<decimal> amount_target_2{ get; set; }
		/// <summary>
		/// 目标音浪-阶段三
		/// </summary>
		public Nullable<decimal> amount_target_3{ get; set; }
		/// <summary>
		/// 目标拉新
		/// </summary>
		public Nullable<int> new_num{ get; set; }
		/// <summary>
		/// 目标二消
		/// </summary>
		public Nullable<decimal> amount_2{ get; set; }
		/// <summary>
		/// 目标二消数
		/// </summary>
		public Nullable<int> num_2{ get; set; }
		/// <summary>
		/// 目标建联
		/// </summary>
		public Nullable<int> contact_num{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-厅战数据 
			/// </summary>	
			public class p_jixiao_tingzhan : ModelDbBase
			{    
							public p_jixiao_tingzhan(){}
				public p_jixiao_tingzhan(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.old_amount =  1m;
														this.amount_2030 =  1m;
														this.amount_2040 =  1m;
														this.amount_2050 =  1m;
														this.amount_2100 =  1m;
														this.amount_2110 =  1m;
														this.amount_2120 =  1m;
														this.amount_2130 =  1m;
														this.amount_2140 =  1m;
														this.amount_2150 =  1m;
														this.amount_2200 =  1m;
														this.amount_2210 =  1m;
														this.amount_2220 =  1m;
														this.amount_2230 =  1m;
														this.amount_2240 =  1m;
														this.amount_2250 =  1m;
														this.amount_2300 =  1m;
														this.amount_2310 =  1m;
														this.amount_2320 =  1m;
														this.amount_2330 =  1m;
														this.amount_2340 =  1m;
														this.amount_2350 =  1m;
														this.amount_2400 =  1m;
														this.amount_total =  1m;
														this.amount_total_day =  1m;
														this.ting_num =  1;
														this.user_type_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 绩效发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 厅战前流水
		/// </summary>
		public Nullable<decimal> old_amount{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2030{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2040{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2050{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2100{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2110{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2120{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2130{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2140{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2150{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2200{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2210{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2220{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2230{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2240{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2250{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2300{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2310{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2320{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2330{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2340{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2350{ get; set; }
		/// <summary>
		/// 音浪值
		/// </summary>
		public Nullable<decimal> amount_2400{ get; set; }
		/// <summary>
		/// 厅战总流水
		/// </summary>
		public Nullable<decimal> amount_total{ get; set; }
		/// <summary>
		/// 厅战当天总流水
		/// </summary>
		public Nullable<decimal> amount_total_day{ get; set; }
		/// <summary>
		/// 参加厅战厅数
		/// </summary>
		public Nullable<int> ting_num{ get; set; }
		/// <summary>
		/// 提交人user_sn
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 用户类型id
		/// </summary>
		public Nullable<int> user_type_id{ get; set; }
			}
	    			/// <summary>
			/// 表实体-绩效-请假 
			/// </summary>	
			public class p_jixiao_vacation : ModelDbBase
			{    
							public p_jixiao_vacation(){}
				public p_jixiao_vacation(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 替档主播用户编号
		/// </summary>
		public string new_zb_user_sn{ get; set; }
		/// <summary>
		/// 请假日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 事由
		/// </summary>
		public string note{ get; set; }
		/// <summary>
		/// 缺档原因
		/// </summary>
		public string cause{ get; set; }
			}
	    			/// <summary>
			/// 表实体-外宣补人-需求 
			/// </summary>	
			public class p_join_apply : ModelDbBase
			{    
							public p_join_apply(){}
				public p_join_apply(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.manager_age =  1;
														this.open_hours =  1;
														this.zb_count =  1;
														this.status =  1;
														this.recruited_count =  1;
														this.finish_zb_count =  1;
														this.training_zb_count =  1;
														this.quit_count =  1;
														this.leave_rate =  1;
														this.weight =  1;
														this.join_rate =  1m;
														this.stay_rate =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 申请单编号(唯一标识)
		/// </summary>
		public string apply_sn{ get; set; }
		/// <summary>
		/// 运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 厅管用户名
		/// </summary>
		public string tg_username{ get; set; }
		/// <summary>
		/// 男女厅（厅信息）
		/// </summary>
		public string tg_sex{ get; set; }
		/// <summary>
		/// 管理
		/// </summary>
		public string manager{ get; set; }
		/// <summary>
		/// 管理年龄
		/// </summary>
		public Nullable<int> manager_age{ get; set; }
		/// <summary>
		/// 开厅时长
		/// </summary>
		public Nullable<int> open_hours{ get; set; }
		/// <summary>
		/// 目前在开档
		/// </summary>
		public string current_open_dangwei{ get; set; }
		/// <summary>
		/// 申请主播人数
		/// </summary>
		public Nullable<int> zb_count{ get; set; }
		/// <summary>
		/// 申请原因
		/// </summary>
		public string apply_cause{ get; set; }
		/// <summary>
		/// 审批状态#enum:等待运营审批=0;等待公会审批=1;等待外宣补人=2;已完成=3;已取消=4;已拒绝=9
		/// </summary>
		public Nullable<int> status{ get; set; }
		public enum status_enum {
			等待运营审批=0,
			等待公会审批=1,
			等待外宣补人=2,
			已完成=3,
			已取消=4,
			已拒绝=9,
		}
		/// <summary>
		/// 已分配人数
		/// </summary>
		public Nullable<int> recruited_count{ get; set; }
		/// <summary>
		/// 已拉群人数
		/// </summary>
		public Nullable<int> finish_zb_count{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Nullable<int> training_zb_count{ get; set; }
		/// <summary>
		/// 流失人数
		/// </summary>
		public Nullable<int> quit_count{ get; set; }
		/// <summary>
		/// 意向年龄
		/// </summary>
		public string age_need{ get; set; }
		/// <summary>
		/// 离职率
		/// </summary>
		public Nullable<int> leave_rate{ get; set; }
		/// <summary>
		/// 权重值
		/// </summary>
		public Nullable<int> weight{ get; set; }
		/// <summary>
		/// 补人率
		/// </summary>
		public Nullable<decimal> join_rate{ get; set; }
		/// <summary>
		/// 留人率
		/// </summary>
		public Nullable<decimal> stay_rate{ get; set; }
			}
	    			/// <summary>
			/// 表实体-外宣补人-需求-申请档位明细 
			/// </summary>	
			public class p_join_apply_item : ModelDbBase
			{    
							public p_join_apply_item(){}
				public p_join_apply_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.zb_count =  1;
														this.unsupplement_count =  1;
														this.recruited_count =  1;
														this.put_count =  1;
														this.finish_zb_count =  1;
														this.training_zb_count =  1;
														this.quit_count =  1;
														this.other_count =  1;
														this.status =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 申请单编号(关联申请单的唯一编号)
		/// </summary>
		public string apply_sn{ get; set; }
		/// <summary>
		/// 申请单档表编号(唯一标识)
		/// </summary>
		public string apply_item_sn{ get; set; }
		/// <summary>
		/// 档位
		/// </summary>
		public string dangwei{ get; set; }
		/// <summary>
		/// 申请主播人数（总人数）
		/// </summary>
		public Nullable<int> zb_count{ get; set; }
		/// <summary>
		/// 未分配人数
		/// </summary>
		public Nullable<int> unsupplement_count{ get; set; }
		/// <summary>
		/// 待入库（已分配人数）
		/// </summary>
		public Nullable<int> recruited_count{ get; set; }
		/// <summary>
		/// 待拉群（已入库人数）
		/// </summary>
		public Nullable<int> put_count{ get; set; }
		/// <summary>
		/// 待培训（已拉群人数）
		/// </summary>
		public Nullable<int> finish_zb_count{ get; set; }
		/// <summary>
		/// 已完成（已培训人数）
		/// </summary>
		public Nullable<int> training_zb_count{ get; set; }
		/// <summary>
		/// 流失人数
		/// </summary>
		public Nullable<int> quit_count{ get; set; }
		/// <summary>
		/// 其他人数
		/// </summary>
		public Nullable<int> other_count{ get; set; }
		/// <summary>
		/// 最近补人时间
		/// </summary>
		public Nullable<DateTime> last_time{ get; set; }
		/// <summary>
		/// 补人完成状态#enum:未完成=0;已完成=1
		/// </summary>
		public Nullable<int> status{ get; set; }
		public enum status_enum {
			未完成=0,
			已完成=1,
		}
			}
	    			/// <summary>
			/// 表实体-补人需求-日志 
			/// </summary>	
			public class p_join_apply_log : ModelDbBase
			{    
							public p_join_apply_log(){}
				public p_join_apply_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 操作类型#enum:补人申请=0;取消=1;删除=2;审批通过=3;运营审批=4;公会审批=5;完成=10;置顶=15
		/// </summary>
		public Nullable<sbyte> c_type{ get; set; }
		public enum c_type_enum {
			补人申请=0,
			取消=1,
			删除=2,
			审批通过=3,
			运营审批=4,
			公会审批=5,
			完成=10,
			置顶=15,
		}
		/// <summary>
		/// 关联p_join_apply的apply_sn唯一键
		/// </summary>
		public string apply_sn{ get; set; }
		/// <summary>
		/// 内容描述
		/// </summary>
		public string content{ get; set; }
		/// <summary>
		/// 操作人用户类型id
		/// </summary>
		public Nullable<int> user_type_id{ get; set; }
		/// <summary>
		/// 操作人
		/// </summary>
		public string user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-转厅申请表 
			/// </summary>	
			public class p_join_change_apply : ModelDbBase
			{    
							public p_join_change_apply(){}
				public p_join_change_apply(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_info_zhubo_id =  1;
														this.status =  1;
														this.is_dy_done =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 主播信息id
		/// </summary>
		public Nullable<int> user_info_zhubo_id{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 主播sn
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// user_info_zb唯一标识
		/// </summary>
		public string user_info_zb_sn{ get; set; }
		/// <summary>
		/// 转入厅管sn
		/// </summary>
		public string t_tg_user_sn{ get; set; }
		/// <summary>
		/// 转入厅sn
		/// </summary>
		public string t_ting_sn{ get; set; }
		/// <summary>
		/// 申请日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 申请记录状态#enum:转厅失败=0;转厅成功=1;等待运营审批=2;申请退回=3,等待对方同意=4
		/// </summary>
		public Nullable<int> status{ get; set; }
		public enum status_enum {
			转厅失败=0,
			转厅成功=1,
			等待运营审批=2,
			申请退回=3,等待对方同意=4,
		}
		/// <summary>
		/// 抖音后台操作是否完成#enum:未完成=0;已完成=1
		/// </summary>
		public Nullable<int> is_dy_done{ get; set; }
		public enum is_dy_done_enum {
			未完成=0,
			已完成=1,
		}
		/// <summary>
		/// 退回原因
		/// </summary>
		public string return_reason{ get; set; }
			}
	    			/// <summary>
			/// 表实体-需求完成 
			/// </summary>	
			public class p_join_finish : ModelDbBase
			{    
							public p_join_finish(){}
				public p_join_finish(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.p_join_need_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 主播编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 主播账号
		/// </summary>
		public string zb_username{ get; set; }
		/// <summary>
		/// 外键:推人需求表id
		/// </summary>
		public Nullable<int> p_join_need_id{ get; set; }
			}
	    			/// <summary>
			/// 表实体-外宣补人-需求 
			/// </summary>	
			public class p_join_need : ModelDbBase
			{    
							public p_join_need(){}
				public p_join_need(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.manager_age =  1;
														this.open_hours =  1;
														this.zb_count =  1;
														this.status =  1;
														this.supplement_count =  1;
														this.inqun_count =  1;
														this.finish_zb_count =  1;
														this.quit_count =  1;
														this.step =  1m;
														this.complete_status =  1;
														this.leave_rate =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 需求编号
		/// </summary>
		public string need_sn{ get; set; }
		/// <summary>
		/// 运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 厅管用户名
		/// </summary>
		public string tg_username{ get; set; }
		/// <summary>
		/// 厅管性别
		/// </summary>
		public string tg_sex{ get; set; }
		/// <summary>
		/// 管理
		/// </summary>
		public string manager{ get; set; }
		/// <summary>
		/// 管理年龄
		/// </summary>
		public Nullable<int> manager_age{ get; set; }
		/// <summary>
		/// 开厅时长
		/// </summary>
		public Nullable<int> open_hours{ get; set; }
		/// <summary>
		/// 目前在开档
		/// </summary>
		public string current_open_dangwei{ get; set; }
		/// <summary>
		/// 申请主播明细(挡位,需求人数count,已分配人数recruited_count,拉群人数inqun_count,流失人数quite)
		/// </summary>
		public string apply_details{ get; set; }
		/// <summary>
		/// 申请主播人数
		/// </summary>
		public Nullable<int> zb_count{ get; set; }
		/// <summary>
		/// 申请原因
		/// </summary>
		public string apply_cause{ get; set; }
		/// <summary>
		/// 运营审批人编号
		/// </summary>
		public string approver_user_sn{ get; set; }
		/// <summary>
		/// 运营审批时间
		/// </summary>
		public Nullable<DateTime> approve_time{ get; set; }
		/// <summary>
		/// 运营审批原因
		/// </summary>
		public string notes{ get; set; }
		/// <summary>
		/// 管理审批人编号
		/// </summary>
		public string m_approver_user_sn{ get; set; }
		/// <summary>
		/// 管理审批时间
		/// </summary>
		public Nullable<DateTime> m_approve_time{ get; set; }
		/// <summary>
		/// 管理审批原因
		/// </summary>
		public string m_notes{ get; set; }
		/// <summary>
		/// 审批状态#enum:等待运营审批=0;等待公会审批=1;等待外宣补人=2;已完成=3;已取消=4;已拒绝=9
		/// </summary>
		public Nullable<int> status{ get; set; }
		public enum status_enum {
			等待运营审批=0,
			等待公会审批=1,
			等待外宣补人=2,
			已完成=3,
			已取消=4,
			已拒绝=9,
		}
		/// <summary>
		/// 已补人数
		/// </summary>
		public Nullable<int> supplement_count{ get; set; }
		/// <summary>
		/// 已拉群人数
		/// </summary>
		public Nullable<int> inqun_count{ get; set; }
		/// <summary>
		/// 已完成创建的主播人数
		/// </summary>
		public Nullable<int> finish_zb_count{ get; set; }
		/// <summary>
		/// 流失人数
		/// </summary>
		public Nullable<int> quit_count{ get; set; }
		/// <summary>
		/// 意向年龄
		/// </summary>
		public string age_need{ get; set; }
		/// <summary>
		/// 取关联节奏表当前阶段
		/// </summary>
		public Nullable<decimal> step{ get; set; }
		/// <summary>
		/// 申请单状态#enum:未完成=0;已完成=1
		/// </summary>
		public Nullable<int> complete_status{ get; set; }
		public enum complete_status_enum {
			未完成=0,
			已完成=1,
		}
		/// <summary>
		/// 离职率
		/// </summary>
		public Nullable<int> leave_rate{ get; set; }
			}
	    			/// <summary>
			/// 表实体-外宣补人-需求-主播 
			/// </summary>	
			public class p_join_need_item : ModelDbBase
			{    
							public p_join_need_item(){}
				public p_join_need_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.dangwei =  1;
														this.count =  1;
														this.recruited_count =  1;
														this.quite_count =  1;
														this.inqun_count =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 需求sn
		/// </summary>
		public string need_sn{ get; set; }
		/// <summary>
		/// 档位
		/// </summary>
		public Nullable<int> dangwei{ get; set; }
		/// <summary>
		/// 需求人数
		/// </summary>
		public Nullable<int> count{ get; set; }
		/// <summary>
		/// 已分配人数
		/// </summary>
		public Nullable<int> recruited_count{ get; set; }
		/// <summary>
		/// 已流失人数
		/// </summary>
		public Nullable<int> quite_count{ get; set; }
		/// <summary>
		/// 已拉群人数
		/// </summary>
		public Nullable<int> inqun_count{ get; set; }
			}
	    			/// <summary>
			/// 表实体-补人需求-日志 
			/// </summary>	
			public class p_join_need_log : ModelDbBase
			{    
							public p_join_need_log(){}
				public p_join_need_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.need_id =  1;
														this.user_type_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 操作类型#enum:取消=1;删除=2;审批通过=3
		/// </summary>
		public Nullable<sbyte> c_type{ get; set; }
		public enum c_type_enum {
			取消=1,
			删除=2,
			审批通过=3,
		}
		/// <summary>
		/// 关联p_join_need的id
		/// </summary>
		public Nullable<int> need_id{ get; set; }
		/// <summary>
		/// 内容描述
		/// </summary>
		public string content{ get; set; }
		/// <summary>
		/// 操作人用户类型id
		/// </summary>
		public Nullable<int> user_type_id{ get; set; }
		/// <summary>
		/// 操作人
		/// </summary>
		public string user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-外宣补人-需求-主播 
			/// </summary>	
			public class p_join_need_zb : ModelDbBase
			{    
							public p_join_need_zb(){}
				public p_join_need_zb(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_info_zb_id =  1;
														this.dangwei =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 对应补人申请表sn
		/// </summary>
		public string need_sn{ get; set; }
		/// <summary>
		/// 关联user_info_zb的id
		/// </summary>
		public Nullable<int> user_info_zb_id{ get; set; }
		/// <summary>
		/// 对应补人申请表档位id
		/// </summary>
		public Nullable<int> dangwei{ get; set; }
		/// <summary>
		/// 对接厅管（所属厅管user_sn） 2
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 上一任对接厅管
		/// </summary>
		public string old_tg_user_sn{ get; set; }
		/// <summary>
		/// 上一任对接厅厅名
		/// </summary>
		public string old_tg_username{ get; set; }
		/// <summary>
		/// 外宣补人状态#enum:等待分配=0;等待入库=1;等待拉群=2;已经流失=3;等待回访=4
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			等待分配=0,
			等待入库=1,
			等待拉群=2,
			已经流失=3,
			等待回访=4,
		}
			}
	    			/// <summary>
			/// 表实体-补人需求-主播信息-日志 
			/// </summary>	
			public class p_join_need_zb_log : ModelDbBase
			{    
							public p_join_need_zb_log(){}
				public p_join_need_zb_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_info_zb_id =  1;
														this.user_type_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 操作类型#enum:分级=0;分配=1;入库=2;拉群=3;流失=4;退回=5
		/// </summary>
		public Nullable<sbyte> c_type{ get; set; }
		public enum c_type_enum {
			分级=0,
			分配=1,
			入库=2,
			拉群=3,
			流失=4,
			退回=5,
		}
		/// <summary>
		/// 关联user_info_zhubo/p_join_new_info的唯一标识
		/// </summary>
		public Nullable<int> user_info_zb_id{ get; set; }
		/// <summary>
		/// 内容描述
		/// </summary>
		public string content{ get; set; }
		/// <summary>
		/// 操作人用户类型id
		/// </summary>
		public Nullable<int> user_type_id{ get; set; }
		/// <summary>
		/// 操作人
		/// </summary>
		public string user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-补人需求-运营团队-补人城市优先级 
			/// </summary>	
			public class p_join_new_citys : ModelDbBase
			{    
							public p_join_new_citys(){}
				public p_join_new_citys(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.priority =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 厅id，多个逗号分隔
		/// </summary>
		public string ting_ids{ get; set; }
		/// <summary>
		/// 城市名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 优先级，越小越高
		/// </summary>
		public Nullable<int> priority{ get; set; }
			}
	    			/// <summary>
			/// 表实体-补人需求-中台地区-补人城市优先级 
			/// </summary>	
			public class p_join_new_citys_zt : ModelDbBase
			{    
							public p_join_new_citys_zt(){}
				public p_join_new_citys_zt(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 中台用户编号
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 城市名称
		/// </summary>
		public string name{ get; set; }
			}
	    			/// <summary>
			/// 表实体-补人-团队数据统计 
			/// </summary>	
			public class p_join_new_data_statistics_yy : ModelDbBase
			{    
							public p_join_new_data_statistics_yy(){}
				public p_join_new_data_statistics_yy(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.zb_sum =  1;
														this.unsupplement_sum =  1;
														this.uninqun_sum =  1;
														this.inqun_sum =  1;
														this.b_allocation_loss =  1;
														this.b_traning_loss =  1;
														this.a_traning_loss =  1;
														this.male_zb_sum =  1;
														this.female_zb_sum =  1;
														this.male_training_sum =  1;
														this.female_training_sum =  1;
														this.male_supplement_sum =  1;
														this.female_supplement_sum =  1;
														this.male_inqun_sum =  1;
														this.female_inqun_sum =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 运营名称
		/// </summary>
		public string yy_name{ get; set; }
		/// <summary>
		/// 补人申请日期
		/// </summary>
		public string apply_date{ get; set; }
		/// <summary>
		/// 申请数
		/// </summary>
		public Nullable<int> zb_sum{ get; set; }
		/// <summary>
		/// 未分配
		/// </summary>
		public Nullable<int> unsupplement_sum{ get; set; }
		/// <summary>
		/// 待拉群
		/// </summary>
		public Nullable<int> uninqun_sum{ get; set; }
		/// <summary>
		/// 已拉群
		/// </summary>
		public Nullable<int> inqun_sum{ get; set; }
		/// <summary>
		/// 分配前流失数
		/// </summary>
		public Nullable<int> b_allocation_loss{ get; set; }
		/// <summary>
		/// 分配后流失数未培训
		/// </summary>
		public Nullable<int> b_traning_loss{ get; set; }
		/// <summary>
		/// 分配后流失数已培训
		/// </summary>
		public Nullable<int> a_traning_loss{ get; set; }
		/// <summary>
		/// 申请数男生
		/// </summary>
		public Nullable<int> male_zb_sum{ get; set; }
		/// <summary>
		/// 申请数女生
		/// </summary>
		public Nullable<int> female_zb_sum{ get; set; }
		/// <summary>
		/// 培训名单总数男生
		/// </summary>
		public Nullable<int> male_training_sum{ get; set; }
		/// <summary>
		/// 培训名单总数女生
		/// </summary>
		public Nullable<int> female_training_sum{ get; set; }
		/// <summary>
		/// 已分配总数男生
		/// </summary>
		public Nullable<int> male_supplement_sum{ get; set; }
		/// <summary>
		/// 已分配总数女生
		/// </summary>
		public Nullable<int> female_supplement_sum{ get; set; }
		/// <summary>
		/// 已拉群总数男生
		/// </summary>
		public Nullable<int> male_inqun_sum{ get; set; }
		/// <summary>
		/// 已拉群总数女生
		/// </summary>
		public Nullable<int> female_inqun_sum{ get; set; }
			}
	    			/// <summary>
			/// 表实体-萌新收集-学员信息 
			/// </summary>	
			public class p_join_new_info : ModelDbBase
			{    
							public p_join_new_info(){}
				public p_join_new_info(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.tg_need_id =  1;
														this.tg_dangwei =  1;
														this.age =  1;
														this.devices_num =  1;
														this.supplement_sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 唯一编号
		/// </summary>
		public string p_join_new_info_sn{ get; set; }
		/// <summary>
		/// 所属主播sn(成为主播前为空)
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 对接厅管（所属厅管user_sn） 2
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 直播厅
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 签约sn 1
		/// </summary>
		public string qy_sn{ get; set; }
		/// <summary>
		/// 所属萌新sn
		/// </summary>
		public string mx_sn{ get; set; }
		/// <summary>
		/// 所属流量sn
		/// </summary>
		public string wx_sn{ get; set; }
		/// <summary>
		/// 上一任对接厅sn
		/// </summary>
		public string old_tg_user_sn{ get; set; }
		/// <summary>
		/// 上一任对接厅厅名
		/// </summary>
		public string old_tg_username{ get; set; }
		/// <summary>
		/// 对应补人申请表id
		/// </summary>
		public Nullable<int> tg_need_id{ get; set; }
		/// <summary>
		/// 对应补人申请表档位id
		/// </summary>
		public Nullable<int> tg_dangwei{ get; set; }
		/// <summary>
		/// 年龄 3
		/// </summary>
		public Nullable<int> age{ get; set; }
		/// <summary>
		/// 是否已婚 3
		/// </summary>
		public string marriage{ get; set; }
		/// <summary>
		/// 有无孩子 3
		/// </summary>
		public string child{ get; set; }
		/// <summary>
		/// 生日 3
		/// </summary>
		public string birthday{ get; set; }
		/// <summary>
		/// 星座 3
		/// </summary>
		public string star_sign{ get; set; }
		/// <summary>
		/// 才艺 3
		/// </summary>
		public string talent{ get; set; }
		/// <summary>
		/// 声卡 3
		/// </summary>
		public string sound_card{ get; set; }
		/// <summary>
		/// 招聘渠道 3
		/// </summary>
		public string way{ get; set; }
		/// <summary>
		/// 电话号码 3
		/// </summary>
		public string mobile{ get; set; }
		/// <summary>
		/// 设备数量 3
		/// </summary>
		public Nullable<int> devices_num{ get; set; }
		/// <summary>
		/// 学历(字典：学历) 3
		/// </summary>
		public Nullable<sbyte> education{ get; set; }
		/// <summary>
		/// mbti人格 3
		/// </summary>
		public string mbti{ get; set; }
		/// <summary>
		/// 是否负债 3
		/// </summary>
		public string is_low{ get; set; }
		/// <summary>
		/// 地区(省市) 123
		/// </summary>
		public string address{ get; set; }
		/// <summary>
		/// 所在省份
		/// </summary>
		public string province{ get; set; }
		/// <summary>
		/// 所在城市
		/// </summary>
		public string city{ get; set; }
		/// <summary>
		/// 直播经验 3
		/// </summary>
		public string experience{ get; set; }
		/// <summary>
		/// 现实工作 23
		/// </summary>
		public string job{ get; set; }
		/// <summary>
		/// 目标收入 3
		/// </summary>
		public string revenue{ get; set; }
		/// <summary>
		/// 性别 12
		/// </summary>
		public string zb_sex{ get; set; }
		/// <summary>
		/// 微信openid
		/// </summary>
		public string openid{ get; set; }
		/// <summary>
		/// 微信昵称 12
		/// </summary>
		public string wechat_nickname{ get; set; }
		/// <summary>
		/// 微信账号2
		/// </summary>
		public string wechat_username{ get; set; }
		/// <summary>
		/// 抖音账号 12
		/// </summary>
		public string dou_username{ get; set; }
		/// <summary>
		/// 抖音昵称 2
		/// </summary>
		public string dou_nickname{ get; set; }
		/// <summary>
		/// 抖音作者id（抖音官方主播唯一身份id）
		/// </summary>
		public string anchor_id{ get; set; }
		/// <summary>
		/// 上传记录(萌新端上传截图)
		/// </summary>
		public string upload_records{ get; set; }
		/// <summary>
		/// 接档时间（字典：档位时段id,多个用逗号隔开） 123
		/// </summary>
		public string sessions{ get; set; }
		/// <summary>
		/// 兼职全职 123
		/// </summary>
		public string full_or_part{ get; set; }
		/// <summary>
		/// 期数
		/// </summary>
		public string term{ get; set; }
		/// <summary>
		/// 主播分级(萌新端在主播入职时划分的等级)
		/// </summary>
		public string zb_level{ get; set; }
		/// <summary>
		/// 主播分级时间
		/// </summary>
		public Nullable<DateTime> zb_level_time{ get; set; }
		/// <summary>
		/// 说明
		/// </summary>
		public string note{ get; set; }
		/// <summary>
		/// 主播质量
		/// </summary>
		public Nullable<sbyte> quality{ get; set; }
		/// <summary>
		/// 分配优先级
		/// </summary>
		public Nullable<int> supplement_sort{ get; set; }
		/// <summary>
		/// 对接群 1
		/// </summary>
		public string qun{ get; set; }
		/// <summary>
		/// 流失原因（萌新操作）
		/// </summary>
		public string no_share{ get; set; }
		/// <summary>
		/// 状态#enum:补人完成=0;等待分级=1;等待分配=2;等待入库=3;等待拉群=4;等待退回=5;暂不分配=6;改抖音号=7;入库失败=8;逻辑删除=9;有对接厅=10;等待培训=11;已经流失=12;中台锁定=13
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			补人完成=0,
			等待分级=1,
			等待分配=2,
			等待入库=3,
			等待拉群=4,
			等待退回=5,
			暂不分配=6,
			改抖音号=7,
			入库失败=8,
			逻辑删除=9,
			有对接厅=10,
			等待培训=11,
			已经流失=12,
			中台锁定=13,
		}
		/// <summary>
		/// 是否加急#enum:不加急=0;加急=1
		/// </summary>
		public Nullable<sbyte> is_fast{ get; set; }
		public enum is_fast_enum {
			不加急=0,
			加急=1,
		}
			}
	    			/// <summary>
			/// 表实体-补人需求-主播信息-日志 
			/// </summary>	
			public class p_join_new_info_log : ModelDbBase
			{    
							public p_join_new_info_log(){}
				public p_join_new_info_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_info_zb_id =  1;
														this.user_type_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 操作类型#enum:资料收集=-1;分级=0;分配=1;入库=2;拉群=3;流失=4;退回=5;恢复分配=6;删除=7;改抖音号=8;入库失败=9;重新入库=10;完成培训=11;中台锁定=13;转移萌新=14
		/// </summary>
		public Nullable<sbyte> c_type{ get; set; }
		public enum c_type_enum {
			资料收集=-1,
			分级=0,
			分配=1,
			入库=2,
			拉群=3,
			流失=4,
			退回=5,
			恢复分配=6,
			删除=7,
			改抖音号=8,
			入库失败=9,
			重新入库=10,
			完成培训=11,
			中台锁定=13,
			转移萌新=14,
		}
		/// <summary>
		/// 操作前状态#enum:无=-1;补人完成=0;等待分级=1;等待分配=2;等待入库=3;等待拉群=4;等待退回=5;暂不分配=6;改抖音号=7;入库失败=8;逻辑删除=9;有对接厅=10;等待培训=11;中台锁定=13
		/// </summary>
		public Nullable<sbyte> last_status{ get; set; }
		public enum last_status_enum {
			无=-1,
			补人完成=0,
			等待分级=1,
			等待分配=2,
			等待入库=3,
			等待拉群=4,
			等待退回=5,
			暂不分配=6,
			改抖音号=7,
			入库失败=8,
			逻辑删除=9,
			有对接厅=10,
			等待培训=11,
			中台锁定=13,
		}
		/// <summary>
		/// 关联p_join_new_info的id
		/// </summary>
		public Nullable<int> user_info_zb_id{ get; set; }
		/// <summary>
		/// 内容描述
		/// </summary>
		public string content{ get; set; }
		/// <summary>
		/// 操作人用户类型id
		/// </summary>
		public Nullable<int> user_type_id{ get; set; }
		/// <summary>
		/// 操作人
		/// </summary>
		public string user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-补人权重 
			/// </summary>	
			public class p_join_new_weight : ModelDbBase
			{    
							public p_join_new_weight(){}
				public p_join_new_weight(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.weight =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 权重类型#enum:运营=1;厅=2
		/// </summary>
		public Nullable<sbyte> w_type{ get; set; }
		public enum w_type_enum {
			运营=1,
			厅=2,
		}
		/// <summary>
		/// 运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 权重值
		/// </summary>
		public Nullable<int> weight{ get; set; }
			}
	    			/// <summary>
			/// 表实体-临时表格-未分配表格 
			/// </summary>	
			public class p_join_noshare : ModelDbBase
			{    
							public p_join_noshare(){}
				public p_join_noshare(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.age =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 期数
		/// </summary>
		public string term{ get; set; }
		/// <summary>
		/// 微信昵称
		/// </summary>
		public string wechat_nickname{ get; set; }
		/// <summary>
		/// 微信账号
		/// </summary>
		public string wechat_username{ get; set; }
		/// <summary>
		/// 抖音账号
		/// </summary>
		public string dou_username{ get; set; }
		/// <summary>
		/// 抖音昵称
		/// </summary>
		public string dou_nickname{ get; set; }
		/// <summary>
		/// 性别
		/// </summary>
		public string zb_sex{ get; set; }
		/// <summary>
		/// 年龄
		/// </summary>
		public Nullable<int> age{ get; set; }
		/// <summary>
		/// 身份
		/// </summary>
		public string job{ get; set; }
		/// <summary>
		/// 地区(省市)
		/// </summary>
		public string address{ get; set; }
		/// <summary>
		/// 接档时间
		/// </summary>
		public string sessions{ get; set; }
		/// <summary>
		/// 兼职全职
		/// </summary>
		public string full_or_part{ get; set; }
		/// <summary>
		/// 流失原因（萌新操作）
		/// </summary>
		public string no_share{ get; set; }
		/// <summary>
		/// 所属萌新sn
		/// </summary>
		public string mx_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-内推申请表 
			/// </summary>	
			public class p_join_push_apply : ModelDbBase
			{    
							public p_join_push_apply(){}
				public p_join_push_apply(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.status =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 主播的唯一编号(关联user_info_zb)
		/// </summary>
		public string user_info_zb_sn{ get; set; }
		/// <summary>
		/// 厅管sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 经纪人后台
		/// </summary>
		public string jjr_name{ get; set; }
		/// <summary>
		/// 抖音账号
		/// </summary>
		public string dy_account{ get; set; }
		/// <summary>
		/// 真实姓名
		/// </summary>
		public string real_name{ get; set; }
		/// <summary>
		/// 手机后四位
		/// </summary>
		public string moblie_lastfour{ get; set; }
		/// <summary>
		/// 主播点位
		/// </summary>
		public string commission_rate{ get; set; }
		/// <summary>
		/// 申请记录状态#enum:申请中=0;申请完成=1;退回=2;申请失败=3
		/// </summary>
		public Nullable<int> status{ get; set; }
		public enum status_enum {
			申请中=0,
			申请完成=1,
			退回=2,
			申请失败=3,
		}
		/// <summary>
		/// 退回原因
		/// </summary>
		public string return_reason{ get; set; }
			}
	    			/// <summary>
			/// 表实体-审核白名单 
			/// </summary>	
			public class p_join_whitelist : ModelDbBase
			{    
							public p_join_whitelist(){}
				public p_join_whitelist(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 白名单用户user_sn
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 厅sn
		/// </summary>
		public string ting_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-跨房 
			/// </summary>	
			public class p_kuafang : ModelDbBase
			{    
							public p_kuafang(){}
				public p_kuafang(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 跨房时间
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 厅管端提报开始时间
		/// </summary>
		public Nullable<DateTime> start_time{ get; set; }
		/// <summary>
		/// 厅管端提报结束时间
		/// </summary>
		public Nullable<DateTime> end_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-跨房-活动发起 
			/// </summary>	
			public class p_kuafang_mate : ModelDbBase
			{    
							public p_kuafang_mate(){}
				public p_kuafang_mate(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.kuafang_id =  1;
														this.amont =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 跨房id
		/// </summary>
		public Nullable<int> kuafang_id{ get; set; }
		/// <summary>
		/// 发起厅管user_sn
		/// </summary>
		public string tg_user_sn1{ get; set; }
		/// <summary>
		/// 发起厅sn
		/// </summary>
		public string ting_sn1{ get; set; }
		/// <summary>
		/// 对方厅管user_sn
		/// </summary>
		public string tg_user_sn2{ get; set; }
		/// <summary>
		/// 对方厅sn
		/// </summary>
		public string ting_sn2{ get; set; }
		/// <summary>
		/// 是否跨团队#enum:是=1;否=0
		/// </summary>
		public Nullable<sbyte> is_open{ get; set; }
		public enum is_open_enum {
			是=1,
			否=0,
		}
		/// <summary>
		/// 目标音浪
		/// </summary>
		public Nullable<decimal> amont{ get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string content{ get; set; }
			}
	    			/// <summary>
			/// 表实体-跨房-活动报名 
			/// </summary>	
			public class p_kuafang_mate_apply : ModelDbBase
			{    
							public p_kuafang_mate_apply(){}
				public p_kuafang_mate_apply(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.kuafang_id =  1;
														this.kuafang_mate_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 跨房id
		/// </summary>
		public Nullable<int> kuafang_id{ get; set; }
		/// <summary>
		/// 跨房活动id
		/// </summary>
		public Nullable<int> kuafang_mate_id{ get; set; }
		/// <summary>
		/// 厅管user_sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 状态#enum:已确认=1;无效=2
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			已确认=1,
			无效=2,
		}
			}
	    			/// <summary>
			/// 表实体-流量-提成 
			/// </summary>	
			public class p_liuliang_commission : ModelDbBase
			{    
							public p_liuliang_commission(){}
				public p_liuliang_commission(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.num =  1;
														this.status =  1;
														this.amount =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 流量用户编号
		/// </summary>
		public string wx_user_sn{ get; set; }
		/// <summary>
		/// 流量用户名
		/// </summary>
		public string wx_name{ get; set; }
		/// <summary>
		/// 绩效年月
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 推出人数
		/// </summary>
		public Nullable<int> num{ get; set; }
		/// <summary>
		/// 个人确认状态#enum:未确认=0;已确认=1
		/// </summary>
		public Nullable<int> status{ get; set; }
		public enum status_enum {
			未确认=0,
			已确认=1,
		}
		/// <summary>
		/// 总提成
		/// </summary>
		public Nullable<decimal> amount{ get; set; }
			}
	    			/// <summary>
			/// 表实体-流量-提成档位 
			/// </summary>	
			public class p_liuliang_commission_rule : ModelDbBase
			{    
							public p_liuliang_commission_rule(){}
				public p_liuliang_commission_rule(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.num_s =  1;
														this.num_e =  1;
														this.amount =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 推出人数起始
		/// </summary>
		public Nullable<int> num_s{ get; set; }
		/// <summary>
		/// 推出人数结束
		/// </summary>
		public Nullable<int> num_e{ get; set; }
		/// <summary>
		/// 提成（推出一人）
		/// </summary>
		public Nullable<decimal> amount{ get; set; }
			}
	    			/// <summary>
			/// 表实体-流量-提报记录 
			/// </summary>	
			public class p_liuliang_info : ModelDbBase
			{    
							public p_liuliang_info(){}
				public p_liuliang_info(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.num =  1;
														this.male_num =  1;
														this.female_num =  1;
														this.wechat_num =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 流量用户编号
		/// </summary>
		public string wx_user_sn{ get; set; }
		/// <summary>
		/// 绩效日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 初审推出
		/// </summary>
		public Nullable<int> num{ get; set; }
		/// <summary>
		/// 推出男生
		/// </summary>
		public Nullable<int> male_num{ get; set; }
		/// <summary>
		/// 推出女生
		/// </summary>
		public Nullable<int> female_num{ get; set; }
		/// <summary>
		/// 添加微信
		/// </summary>
		public Nullable<int> wechat_num{ get; set; }
			}
	    			/// <summary>
			/// 表实体-流量-目标 
			/// </summary>	
			public class p_liuliang_target : ModelDbBase
			{    
							public p_liuliang_target(){}
				public p_liuliang_target(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.num =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 流量用户编号
		/// </summary>
		public string wx_user_sn{ get; set; }
		/// <summary>
		/// 绩效年月
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 推出人数
		/// </summary>
		public Nullable<int> num{ get; set; }
			}
	    			/// <summary>
			/// 表实体-直播间主播使用情况 
			/// </summary>	
			public class p_live_room_usage : ModelDbBase
			{    
							public p_live_room_usage(){}
				public p_live_room_usage(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.live_room_id =  1;
														this.operation_type =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属中台
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 主播sn
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 直播间ID
		/// </summary>
		public Nullable<int> live_room_id{ get; set; }
		/// <summary>
		/// 操作类型 (1-分配, 2-解除, 3-转移)
		/// </summary>
		public Nullable<int> operation_type{ get; set; }
		/// <summary>
		/// 使用开始时间
		/// </summary>
		public Nullable<DateTime> begin_time{ get; set; }
		/// <summary>
		/// 使用结束时间
		/// </summary>
		public Nullable<DateTime> end_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-直播间-信息 
			/// </summary>	
			public class p_liveroom : ModelDbBase
			{    
							public p_liveroom(){}
				public p_liveroom(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.area_id =  1;
														this.liveroom_type_id =  1;
														this.sort =  1;
														this.iszhibo =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属中台
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 区域id
		/// </summary>
		public Nullable<int> area_id{ get; set; }
		/// <summary>
		/// 主播sn
		/// </summary>
		public string zb_user_sn1{ get; set; }
		/// <summary>
		/// 主播sn
		/// </summary>
		public string zb_user_sn2{ get; set; }
		/// <summary>
		/// 师父名字
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 直播间类型
		/// </summary>
		public Nullable<int> liveroom_type_id{ get; set; }
		/// <summary>
		/// 排序号，越小越靠前
		/// </summary>
		public Nullable<int> sort{ get; set; }
		/// <summary>
		/// 状态#enum:空闲=0;占用=1;预订=2
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			空闲=0,
			占用=1,
			预订=2,
		}
		/// <summary>
		/// 是否直播间
		/// </summary>
		public Nullable<int> iszhibo{ get; set; }
			}
	    			/// <summary>
			/// 表实体-直播间-区域 
			/// </summary>	
			public class p_liveroom_area : ModelDbBase
			{    
							public p_liveroom_area(){}
				public p_liveroom_area(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.parent_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属中台
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 父级id
		/// </summary>
		public Nullable<int> parent_id{ get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 排序号，越小越靠前
		/// </summary>
		public Nullable<int> sort{ get; set; }
			}
	    			/// <summary>
			/// 表实体-直播间位置定位 
			/// </summary>	
			public class p_liveroom_fix : ModelDbBase
			{    
							public p_liveroom_fix(){}
				public p_liveroom_fix(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.area_id =  1;
														this.x =  1;
														this.width =  1;
														this.height =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属中台
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 区域Id
		/// </summary>
		public Nullable<int> area_id{ get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 坐标X 轴
		/// </summary>
		public Nullable<int> x{ get; set; }
		/// <summary>
		/// 坐标Y 轴
		/// </summary>
		public string y{ get; set; }
		/// <summary>
		/// 宽度
		/// </summary>
		public Nullable<int> width{ get; set; }
		/// <summary>
		/// 高度
		/// </summary>
		public Nullable<int> height{ get; set; }
		/// <summary>
		/// 边框颜色
		/// </summary>
		public string borderColor{ get; set; }
		/// <summary>
		/// 背景颜色
		/// </summary>
		public string backgroundColor{ get; set; }
			}
	    			/// <summary>
			/// 表实体-直播间操作-日志 
			/// </summary>	
			public class p_liveroom_log : ModelDbBase
			{    
							public p_liveroom_log(){}
				public p_liveroom_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 操作类型状态#enum:直播间=1;直播间区域=2;主播=3
		/// </summary>
		public Nullable<sbyte> c_type{ get; set; }
		public enum c_type_enum {
			直播间=1,
			直播间区域=2,
			主播=3,
		}
		/// <summary>
		/// 中台user_sn
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 操作内容
		/// </summary>
		public string content{ get; set; }
			}
	    			/// <summary>
			/// 表实体-直播间-使用率 
			/// </summary>	
			public class p_liveroom_userate : ModelDbBase
			{    
							public p_liveroom_userate(){}
				public p_liveroom_userate(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.status =  1;
														this.live_room_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属中台
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 状态#enum:空闲=0;占用=1
		/// </summary>
		public Nullable<int> status{ get; set; }
		public enum status_enum {
			空闲=0,
			占用=1,
		}
		/// <summary>
		/// 直播间id
		/// </summary>
		public Nullable<int> live_room_id{ get; set; }
		/// <summary>
		/// 直播间名字，冗余字段，只做记录
		/// </summary>
		public string live_room_name{ get; set; }
		/// <summary>
		/// 排序号，越小越靠前
		/// </summary>
		public Nullable<int> sort{ get; set; }
			}
	    			/// <summary>
			/// 表实体-萌新汇总表 
			/// </summary>	
			public class p_mengxin : ModelDbBase
			{    
							public p_mengxin(){}
				public p_mengxin(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.join_num =  1;
														this.group_num =  1;
														this.in_group_1 =  1;
														this.in_class_1 =  1;
														this.playback_1 =  1;
														this.leave_group_1 =  1;
														this.before_class_2 =  1;
														this.in_group_2 =  1;
														this.in_class_2 =  1;
														this.playback_2 =  1;
														this.leave_group_2 =  1;
														this.in_group_3 =  1;
														this.leave_group_3 =  1;
														this.first_employ =  1;
														this.resit_num =  1;
														this.ignore_num =  1;
														this.no_exam_num =  1;
														this.job_num_ed =  1;
														this.no_job_num =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 拉群期数
		/// </summary>
		public string term{ get; set; }
		/// <summary>
		/// 培训老师
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 拉群日期
		/// </summary>
		public Nullable<DateTime> date{ get; set; }
		/// <summary>
		/// 入会人数
		/// </summary>
		public Nullable<int> join_num{ get; set; }
		/// <summary>
		/// 拉群人数
		/// </summary>
		public Nullable<int> group_num{ get; set; }
		/// <summary>
		/// 第一天在群人数
		/// </summary>
		public Nullable<int> in_group_1{ get; set; }
		/// <summary>
		/// 第一天到课人数
		/// </summary>
		public Nullable<int> in_class_1{ get; set; }
		/// <summary>
		/// 第一天回放人数
		/// </summary>
		public Nullable<int> playback_1{ get; set; }
		/// <summary>
		/// 第一天退群人数
		/// </summary>
		public Nullable<int> leave_group_1{ get; set; }
		/// <summary>
		/// 第二天课前人数
		/// </summary>
		public Nullable<int> before_class_2{ get; set; }
		/// <summary>
		/// 第二天课后人数（原第二天在群人数）
		/// </summary>
		public Nullable<int> in_group_2{ get; set; }
		/// <summary>
		/// 第二天到课人数
		/// </summary>
		public Nullable<int> in_class_2{ get; set; }
		/// <summary>
		/// 第二天回放人数
		/// </summary>
		public Nullable<int> playback_2{ get; set; }
		/// <summary>
		/// 第二天退群人数
		/// </summary>
		public Nullable<int> leave_group_2{ get; set; }
		/// <summary>
		/// 第三天在群人数
		/// </summary>
		public Nullable<int> in_group_3{ get; set; }
		/// <summary>
		/// 第三天退群人数
		/// </summary>
		public Nullable<int> leave_group_3{ get; set; }
		/// <summary>
		/// 首次推出人数
		/// </summary>
		public Nullable<int> first_employ{ get; set; }
		/// <summary>
		/// 补考人数
		/// </summary>
		public Nullable<int> resit_num{ get; set; }
		/// <summary>
		/// 在群不理人数
		/// </summary>
		public Nullable<int> ignore_num{ get; set; }
		/// <summary>
		/// 理了没考人数
		/// </summary>
		public Nullable<int> no_exam_num{ get; set; }
		/// <summary>
		/// 已分配人数
		/// </summary>
		public Nullable<int> job_num_ed{ get; set; }
		/// <summary>
		/// 未分配人数
		/// </summary>
		public Nullable<int> no_job_num{ get; set; }
			}
	    			/// <summary>
			/// 表实体-签约-提成 
			/// </summary>	
			public class p_qianyue_commission : ModelDbBase
			{    
							public p_qianyue_commission(){}
				public p_qianyue_commission(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.wechat_num =  1;
														this.f_num =  1;
														this.qianyue_rate =  1m;
														this.status =  1;
														this.amount =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 签约用户编号
		/// </summary>
		public string qy_user_sn{ get; set; }
		/// <summary>
		/// 签约用户名
		/// </summary>
		public string qy_name{ get; set; }
		/// <summary>
		/// 绩效年月
		/// </summary>
		public string yearmonth{ get; set; }
		/// <summary>
		/// 添加微信
		/// </summary>
		public Nullable<int> wechat_num{ get; set; }
		/// <summary>
		/// 签约人数
		/// </summary>
		public Nullable<int> f_num{ get; set; }
		/// <summary>
		/// 签约率
		/// </summary>
		public Nullable<decimal> qianyue_rate{ get; set; }
		/// <summary>
		/// 个人确认状态#enum:未确认=0;已确认=1
		/// </summary>
		public Nullable<int> status{ get; set; }
		public enum status_enum {
			未确认=0,
			已确认=1,
		}
		/// <summary>
		/// 总提成
		/// </summary>
		public Nullable<decimal> amount{ get; set; }
			}
	    			/// <summary>
			/// 表实体-签约-提成档位 
			/// </summary>	
			public class p_qianyue_commission_rule : ModelDbBase
			{    
							public p_qianyue_commission_rule(){}
				public p_qianyue_commission_rule(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.rate_s =  1;
														this.rate_e =  1;
														this.amount =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 签约率起始
		/// </summary>
		public Nullable<int> rate_s{ get; set; }
		/// <summary>
		/// 签约率结束
		/// </summary>
		public Nullable<int> rate_e{ get; set; }
		/// <summary>
		/// 提成（签约一人）
		/// </summary>
		public Nullable<decimal> amount{ get; set; }
			}
	    			/// <summary>
			/// 表实体-签约-提报记录 
			/// </summary>	
			public class p_qianyue_info : ModelDbBase
			{    
							public p_qianyue_info(){}
				public p_qianyue_info(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.wechat_num =  1;
														this.qianyue_male =  1;
														this.qianyue_female =  1;
														this.f_num =  1;
														this.qianyue_rate =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 签约用户编号
		/// </summary>
		public string qy_user_sn{ get; set; }
		/// <summary>
		/// 绩效日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 添加微信
		/// </summary>
		public Nullable<int> wechat_num{ get; set; }
		/// <summary>
		/// 签约男生
		/// </summary>
		public Nullable<int> qianyue_male{ get; set; }
		/// <summary>
		/// 签约女生
		/// </summary>
		public Nullable<int> qianyue_female{ get; set; }
		/// <summary>
		/// 签约人数
		/// </summary>
		public Nullable<int> f_num{ get; set; }
		/// <summary>
		/// 签约率
		/// </summary>
		public Nullable<decimal> qianyue_rate{ get; set; }
			}
	    			/// <summary>
			/// 表实体-签约名单 
			/// </summary>	
			public class p_qianyue_roster : ModelDbBase
			{    
							public p_qianyue_roster(){}
				public p_qianyue_roster(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.status =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 签约用户编号
		/// </summary>
		public string qy_user_sn{ get; set; }
		/// <summary>
		/// 作者id
		/// </summary>
		public string anchor_id{ get; set; }
		/// <summary>
		/// 经纪人
		/// </summary>
		public string jjr_name{ get; set; }
		/// <summary>
		/// 微信名
		/// </summary>
		public string wx_account{ get; set; }
		/// <summary>
		/// 抖音号
		/// </summary>
		public string dou_account{ get; set; }
		/// <summary>
		/// 抖音昵称
		/// </summary>
		public string dou_nickname{ get; set; }
		/// <summary>
		/// 兼职全职
		/// </summary>
		public string full_or_part{ get; set; }
		/// <summary>
		/// 接档时间（字典：档位时段id,多个用逗号隔开）
		/// </summary>
		public string sessions{ get; set; }
		/// <summary>
		/// 性别
		/// </summary>
		public string sex{ get; set; }
		/// <summary>
		/// 互动（现在不填）
		/// </summary>
		public Nullable<sbyte> is_hudong{ get; set; }
		/// <summary>
		/// 姓名
		/// </summary>
		public string real_name{ get; set; }
		/// <summary>
		/// 手机号后四位
		/// </summary>
		public string mobile_last_four{ get; set; }
		/// <summary>
		/// 期数（培训群）
		/// </summary>
		public string term{ get; set; }
		/// <summary>
		/// 是否进群（现在不填）
		/// </summary>
		public Nullable<sbyte> is_qun{ get; set; }
		/// <summary>
		/// 签约日期
		/// </summary>
		public Nullable<DateTime> qy_date{ get; set; }
		/// <summary>
		/// （用不到这个字段）
		/// </summary>
		public string qun{ get; set; }
		/// <summary>
		/// 签约状态#enum:已签约=0;已退会=1
		/// </summary>
		public Nullable<int> status{ get; set; }
		public enum status_enum {
			已签约=0,
			已退会=1,
		}
			}
	    			/// <summary>
			/// 表实体-任务-扫脸-主播已扫记录 
			/// </summary>	
			public class p_renwu_saolian : ModelDbBase
			{    
							public p_renwu_saolian(){}
				public p_renwu_saolian(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.s_days =  1;
														this.s_hours =  1;
														this.s_amount =  1;
														this.down_num =  1;
														this.saoed_num =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 考核月份
		/// </summary>
		public string c_month{ get; set; }
		/// <summary>
		/// 所属中台用户编号
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string zb_user_sn{ get; set; }
		/// <summary>
		/// 所属厅编号
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 本月有效天数
		/// </summary>
		public Nullable<int> s_days{ get; set; }
		/// <summary>
		/// 本月有效时长
		/// </summary>
		public Nullable<int> s_hours{ get; set; }
		/// <summary>
		/// 本月任务流水
		/// </summary>
		public Nullable<int> s_amount{ get; set; }
		/// <summary>
		/// 扫脸下发次数
		/// </summary>
		public Nullable<int> down_num{ get; set; }
		/// <summary>
		/// 已扫次数
		/// </summary>
		public Nullable<int> saoed_num{ get; set; }
		/// <summary>
		/// 0=无；1=考核无异常
		/// </summary>
		public Nullable<sbyte> s_status{ get; set; }
		/// <summary>
		/// 最后更新时间
		/// </summary>
		public Nullable<DateTime> last_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-任务-扫脸-单厅月份主表 
			/// </summary>	
			public class p_renwu_saolian_tg : ModelDbBase
			{    
							public p_renwu_saolian_tg(){}
				public p_renwu_saolian_tg(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 考核月份
		/// </summary>
		public string c_month{ get; set; }
		/// <summary>
		/// 所属中台用户编号
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 最后查看时间
		/// </summary>
		public Nullable<DateTime> last_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-服务-反馈 
			/// </summary>	
			public class p_service_feedback : ModelDbBase
			{    
							public p_service_feedback(){}
				public p_service_feedback(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属主播用户编号
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 反馈
		/// </summary>
		public string feedback{ get; set; }
		/// <summary>
		/// 联系方式
		/// </summary>
		public string info{ get; set; }
		/// <summary>
		/// 图片url
		/// </summary>
		public string pic_url{ get; set; }
		/// <summary>
		/// 反馈类型#enum:意见反馈=0;倾诉箱=1;推荐=2
		/// </summary>
		public Nullable<sbyte> feedback_type{ get; set; }
		public enum feedback_type_enum {
			意见反馈=0,
			倾诉箱=1,
			推荐=2,
		}
		/// <summary>
		/// 匿名提交#enum:否=0;是=1
		/// </summary>
		public Nullable<sbyte> hidename{ get; set; }
		public enum hidename_enum {
			否=0,
			是=1,
		}
			}
	    			/// <summary>
			/// 表实体-租户-配置 
			/// </summary>	
			public class p_tenant_config : ModelDbBase
			{    
							public p_tenant_config(){}
				public p_tenant_config(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 扫脸数据更新时间
		/// </summary>
		public Nullable<DateTime> saolian_last_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-厅战 
			/// </summary>	
			public class p_tingzhan : ModelDbBase
			{    
							public p_tingzhan(){}
				public p_tingzhan(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.ting_count =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 绩效发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 厅管端提报开始时间
		/// </summary>
		public Nullable<DateTime> start_time{ get; set; }
		/// <summary>
		/// 厅管端提报结束时间
		/// </summary>
		public Nullable<DateTime> end_time{ get; set; }
		/// <summary>
		/// 总厅数
		/// </summary>
		public Nullable<int> ting_count{ get; set; }
			}
	    			/// <summary>
			/// 表实体-厅战-配对 
			/// </summary>	
			public class p_tingzhan_mate : ModelDbBase
			{    
							public p_tingzhan_mate(){}
				public p_tingzhan_mate(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.tingzhan_id =  1;
														this.amont =  1m;
														this.score_1 =  1m;
														this.score_2 =  1m;
														this.score_1_1 =  1m;
														this.score_1_2 =  1m;
														this.score_2_1 =  1m;
														this.score_2_2 =  1m;
														this.score_3_1 =  1m;
														this.score_3_2 =  1m;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 厅战id
		/// </summary>
		public Nullable<int> tingzhan_id{ get; set; }
		/// <summary>
		/// 厅管user_sn左
		/// </summary>
		public string tg_user_sn1{ get; set; }
		/// <summary>
		/// 厅sn左
		/// </summary>
		public string ting_sn1{ get; set; }
		/// <summary>
		/// 厅管user_sn右
		/// </summary>
		public string tg_user_sn2{ get; set; }
		/// <summary>
		/// 厅sn右
		/// </summary>
		public string ting_sn2{ get; set; }
		/// <summary>
		/// 目标音浪
		/// </summary>
		public Nullable<decimal> amont{ get; set; }
		/// <summary>
		/// A厅战绩（单位：万）总分
		/// </summary>
		public Nullable<decimal> score_1{ get; set; }
		/// <summary>
		/// B厅战绩（单位：万）总分
		/// </summary>
		public Nullable<decimal> score_2{ get; set; }
		/// <summary>
		/// A厅战绩（单位：万）第一局
		/// </summary>
		public Nullable<decimal> score_1_1{ get; set; }
		/// <summary>
		/// B厅战绩（单位：万）第一局
		/// </summary>
		public Nullable<decimal> score_1_2{ get; set; }
		/// <summary>
		/// A厅战绩（单位：万）第二局
		/// </summary>
		public Nullable<decimal> score_2_1{ get; set; }
		/// <summary>
		/// B厅战绩（单位：万）第二局
		/// </summary>
		public Nullable<decimal> score_2_2{ get; set; }
		/// <summary>
		/// A厅战绩（单位：万）第三局
		/// </summary>
		public Nullable<decimal> score_3_1{ get; set; }
		/// <summary>
		/// B厅战绩（单位：万）第三局
		/// </summary>
		public Nullable<decimal> score_3_2{ get; set; }
		/// <summary>
		/// 惩罚内容
		/// </summary>
		public string cf_content{ get; set; }
		/// <summary>
		/// 排序号
		/// </summary>
		public Nullable<int> sort{ get; set; }
			}
	    			/// <summary>
			/// 表实体-厅战-指定匹配对手规则 
			/// </summary>	
			public class p_tingzhan_mate_rule : ModelDbBase
			{    
							public p_tingzhan_mate_rule(){}
				public p_tingzhan_mate_rule(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.tingzhan_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 厅战id
		/// </summary>
		public Nullable<int> tingzhan_id{ get; set; }
		/// <summary>
		/// 规则类型#enum:跟厅打=1;不跟厅打=2
		/// </summary>
		public Nullable<sbyte> rule_type{ get; set; }
		public enum rule_type_enum {
			跟厅打=1,
			不跟厅打=2,
		}
		/// <summary>
		/// 厅管user_sn发起
		/// </summary>
		public string tg_user_sn1{ get; set; }
		/// <summary>
		/// 厅sn发起
		/// </summary>
		public string ting_sn1{ get; set; }
		/// <summary>
		/// 厅管user_sn对方
		/// </summary>
		public string tg_user_sn2{ get; set; }
		/// <summary>
		/// 厅sn对方
		/// </summary>
		public string ting_sn2{ get; set; }
			}
	    			/// <summary>
			/// 表实体-厅战-指定匹配对手规则-长期 
			/// </summary>	
			public class p_tingzhan_mate_rulelong : ModelDbBase
			{    
							public p_tingzhan_mate_rulelong(){}
				public p_tingzhan_mate_rulelong(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 规则类型#enum:厅不跟厅打=1;厅不跟运营打=2;运营不跟运营打=3
		/// </summary>
		public Nullable<sbyte> rulelong_type{ get; set; }
		public enum rulelong_type_enum {
			厅不跟厅打=1,
			厅不跟运营打=2,
			运营不跟运营打=3,
		}
		/// <summary>
		/// 发起方user_sn
		/// </summary>
		public string user_sn1{ get; set; }
		/// <summary>
		/// 对方user_sn
		/// </summary>
		public string user_sn2{ get; set; }
		/// <summary>
		/// 状态#enum:禁用=0;启用=1
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			禁用=0,
			启用=1,
		}
			}
	    			/// <summary>
			/// 表实体-厅战 
			/// </summary>	
			public class p_tingzhan_target : ModelDbBase
			{    
							public p_tingzhan_target(){}
				public p_tingzhan_target(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.tingzhan_id =  1;
														this.amont =  1m;
														this.day_amount =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 厅战id
		/// </summary>
		public Nullable<int> tingzhan_id{ get; set; }
		/// <summary>
		/// 运营user_sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 厅管user_sn
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 目标音浪
		/// </summary>
		public Nullable<decimal> amont{ get; set; }
		/// <summary>
		/// 日均音浪（从excel表中导入）
		/// </summary>
		public Nullable<decimal> day_amount{ get; set; }
		/// <summary>
		/// 不参加原因
		/// </summary>
		public string reason{ get; set; }
		/// <summary>
		/// 提交人类型#enum:厅管=1;运营=2
		/// </summary>
		public Nullable<sbyte> creator_type{ get; set; }
		public enum creator_type_enum {
			厅管=1,
			运营=2,
		}
			}
	    			/// <summary>
			/// 表实体-工作-待办-明细 
			/// </summary>	
			public class p_work_todo : ModelDbBase
			{    
							public p_work_todo(){}
				public p_work_todo(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.rule_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 待办开始时间
		/// </summary>
		public Nullable<DateTime> s_date_time{ get; set; }
		/// <summary>
		/// 待办结束时间
		/// </summary>
		public Nullable<DateTime> e_date_time{ get; set; }
		/// <summary>
		/// 实际完成时间
		/// </summary>
		public Nullable<DateTime> f_date_time{ get; set; }
		/// <summary>
		/// 所属中台sn
		/// </summary>
		public string zt_sn{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_sn{ get; set; }
		/// <summary>
		/// 所属厅管sn
		/// </summary>
		public string tg_sn{ get; set; }
		/// <summary>
		/// 规则id,0为自定义
		/// </summary>
		public Nullable<int> rule_id{ get; set; }
		/// <summary>
		/// 显示图标
		/// </summary>
		public string icon{ get; set; }
		/// <summary>
		/// 优先级
		/// </summary>
		public string level{ get; set; }
		/// <summary>
		/// 待办内容
		/// </summary>
		public string content{ get; set; }
		/// <summary>
		/// 排序号，越小越靠前
		/// </summary>
		public Nullable<int> sort{ get; set; }
		/// <summary>
		/// 状态#enum:未完成=0;已完成=1;已取消=2
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			未完成=0,
			已完成=1,
			已取消=2,
		}
			}
	    			/// <summary>
			/// 表实体-工作-待办-规则 
			/// </summary>	
			public class p_work_todo_rule : ModelDbBase
			{    
							public p_work_todo_rule(){}
				public p_work_todo_rule(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属中台sn,如果为空则代表所有中台通用
		/// </summary>
		public string zt_sn{ get; set; }
		/// <summary>
		/// 所属运营sn,如果为空则代表所有运营通用
		/// </summary>
		public string yy_sn{ get; set; }
		/// <summary>
		/// 所属厅管sn,如果为空则代表所有厅管通用
		/// </summary>
		public string tg_sn{ get; set; }
		/// <summary>
		/// 待办内容
		/// </summary>
		public string content{ get; set; }
		/// <summary>
		/// 排序号，越小越靠前
		/// </summary>
		public Nullable<int> sort{ get; set; }
		/// <summary>
		/// 显示图标
		/// </summary>
		public string icon{ get; set; }
		/// <summary>
		/// 优先级
		/// </summary>
		public string level{ get; set; }
		/// <summary>
		/// 重复规则：每天(每周，每月),具体日期
		/// </summary>
		public string c_rule{ get; set; }
		/// <summary>
		/// 开始时间
		/// </summary>
		public Nullable<DateTime> s_time{ get; set; }
		/// <summary>
		/// 结束时间
		/// </summary>
		public Nullable<DateTime> e_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-外宣-线下模式-每日数据 
			/// </summary>	
			public class p_wx_xianxia : ModelDbBase
			{    
							public p_wx_xianxia(){}
				public p_wx_xianxia(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.wokanguo =  1;
														this.kanguowo =  1;
														this.greet =  1;
														this.new_greet =  1;
														this.communicate =  1;
														this.exchange =  1;
														this.wechat_num =  1;
														this.interview_appoint =  1;
														this.interview =  1;
														this.training =  1;
														this.qianyue =  1;
														this.ting_male =  1;
														this.ting_female =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 外宣联系人
		/// </summary>
		public string wx_user_sn{ get; set; }
		/// <summary>
		/// 我看过
		/// </summary>
		public Nullable<int> wokanguo{ get; set; }
		/// <summary>
		/// 看过我
		/// </summary>
		public Nullable<int> kanguowo{ get; set; }
		/// <summary>
		/// 打招呼
		/// </summary>
		public Nullable<int> greet{ get; set; }
		/// <summary>
		/// 牛人新招呼
		/// </summary>
		public Nullable<int> new_greet{ get; set; }
		/// <summary>
		/// 我沟通
		/// </summary>
		public Nullable<int> communicate{ get; set; }
		/// <summary>
		/// 交换电话微信
		/// </summary>
		public Nullable<int> exchange{ get; set; }
		/// <summary>
		/// 已添加微信
		/// </summary>
		public Nullable<int> wechat_num{ get; set; }
		/// <summary>
		/// 预约面试
		/// </summary>
		public Nullable<int> interview_appoint{ get; set; }
		/// <summary>
		/// 已面试
		/// </summary>
		public Nullable<int> interview{ get; set; }
		/// <summary>
		/// 培训
		/// </summary>
		public Nullable<int> training{ get; set; }
		/// <summary>
		/// 签约
		/// </summary>
		public Nullable<int> qianyue{ get; set; }
		/// <summary>
		/// 入男厅
		/// </summary>
		public Nullable<int> ting_male{ get; set; }
		/// <summary>
		/// 入女厅
		/// </summary>
		public Nullable<int> ting_female{ get; set; }
			}
	    			/// <summary>
			/// 表实体-外宣-线下模式-抖音-每日数据 
			/// </summary>	
			public class p_wx_xianxia_douyin : ModelDbBase
			{    
							public p_wx_xianxia_douyin(){}
				public p_wx_xianxia_douyin(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.living_visitors =  1;
														this.living_audience =  1;
														this.new_followers =  1;
														this.wechat_num =  1;
														this.interview_appoint =  1;
														this.interview =  1;
														this.training =  1;
														this.qianyue =  1;
														this.ting_male =  1;
														this.ting_female =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 外宣联系人
		/// </summary>
		public string wx_user_sn{ get; set; }
		/// <summary>
		/// 直播场次
		/// </summary>
		public string living_session{ get; set; }
		/// <summary>
		/// 直播时间
		/// </summary>
		public string living_schedule{ get; set; }
		/// <summary>
		/// 进房人数
		/// </summary>
		public Nullable<int> living_visitors{ get; set; }
		/// <summary>
		/// 曝光人数
		/// </summary>
		public Nullable<int> living_audience{ get; set; }
		/// <summary>
		/// 新增粉丝数
		/// </summary>
		public Nullable<int> new_followers{ get; set; }
		/// <summary>
		/// 已添加微信
		/// </summary>
		public Nullable<int> wechat_num{ get; set; }
		/// <summary>
		/// 预约面试
		/// </summary>
		public Nullable<int> interview_appoint{ get; set; }
		/// <summary>
		/// 已面试
		/// </summary>
		public Nullable<int> interview{ get; set; }
		/// <summary>
		/// 培训
		/// </summary>
		public Nullable<int> training{ get; set; }
		/// <summary>
		/// 签约
		/// </summary>
		public Nullable<int> qianyue{ get; set; }
		/// <summary>
		/// 入男厅
		/// </summary>
		public Nullable<int> ting_male{ get; set; }
		/// <summary>
		/// 入女厅
		/// </summary>
		public Nullable<int> ting_female{ get; set; }
			}
	    			/// <summary>
			/// 表实体-线下数据-每日数据 
			/// </summary>	
			public class p_xianxia_day : ModelDbBase
			{    
							public p_xianxia_day(){}
				public p_xianxia_day(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.zp_channel =  1;
														this.post_num =  1;
														this.exchange =  1;
														this.wechat_num =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 外宣联系人
		/// </summary>
		public string wx_user_sn{ get; set; }
		/// <summary>
		/// 中台sn，地区
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 招聘渠道
		/// </summary>
		public Nullable<int> zp_channel{ get; set; }
		/// <summary>
		/// 发生日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 发布职位数
		/// </summary>
		public Nullable<int> post_num{ get; set; }
		/// <summary>
		/// 交换微信数
		/// </summary>
		public Nullable<int> exchange{ get; set; }
		/// <summary>
		/// 添加微信数
		/// </summary>
		public Nullable<int> wechat_num{ get; set; }
			}
	    			/// <summary>
			/// 表实体-外宣-线下直聘-简历可选字段配置 
			/// </summary>	
			public class p_xianxiazp_filed : ModelDbBase
			{    
							public p_xianxiazp_filed(){}
				public p_xianxiazp_filed(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 基地sn
		/// </summary>
		public string jd_user_sn{ get; set; }
		/// <summary>
		/// 可选字段名，多个用逗号隔开
		/// </summary>
		public string fileds{ get; set; }
			}
	    			/// <summary>
			/// 表实体-外宣-线下直聘-人员信息 
			/// </summary>	
			public class p_xianxiazp_info : ModelDbBase
			{    
							public p_xianxiazp_info(){}
				public p_xianxiazp_info(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.zp_channel =  1;
														this.jianli_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 流量用户sn，邀约人
		/// </summary>
		public string wx_user_sn{ get; set; }
		/// <summary>
		/// 基地sn
		/// </summary>
		public string jd_user_sn{ get; set; }
		/// <summary>
		/// 中台sn，面试官/对接人
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 招聘渠道
		/// </summary>
		public Nullable<int> zp_channel{ get; set; }
		/// <summary>
		/// 邀约日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 微信名
		/// </summary>
		public string wechat_username{ get; set; }
		/// <summary>
		/// 微信号
		/// </summary>
		public string wechat_user_id{ get; set; }
		/// <summary>
		/// 性别
		/// </summary>
		public string gender{ get; set; }
		/// <summary>
		/// 预约面试日期
		/// </summary>
		public Nullable<DateTime> yy_interview_date{ get; set; }
		/// <summary>
		/// 简历id
		/// </summary>
		public Nullable<int> jianli_id{ get; set; }
		/// <summary>
		/// 实际面试日期
		/// </summary>
		public Nullable<DateTime> interview_date{ get; set; }
		/// <summary>
		/// 面试官意见
		/// </summary>
		public string interviewer_feedback{ get; set; }
		/// <summary>
		/// 面试结果#enum:未选择=0;不通过=1;通过=2
		/// </summary>
		public Nullable<sbyte> interview_result{ get; set; }
		public enum interview_result_enum {
			未选择=0,
			不通过=1,
			通过=2,
		}
		/// <summary>
		/// 预约入职日期
		/// </summary>
		public Nullable<DateTime> yy_ruzhi_date{ get; set; }
		/// <summary>
		/// 实际入职日期
		/// </summary>
		public Nullable<DateTime> ruzhi_date{ get; set; }
		/// <summary>
		/// 入职厅sn
		/// </summary>
		public string ting_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-外宣-线下直聘-简历 
			/// </summary>	
			public class p_xianxiazp_jianli : ModelDbBase
			{    
							public p_xianxiazp_jianli(){}
				public p_xianxiazp_jianli(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 流量用户sn，邀约人
		/// </summary>
		public string wx_user_sn{ get; set; }
		/// <summary>
		/// 应聘人姓名
		/// </summary>
		public string username{ get; set; }
		/// <summary>
		/// 面试日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 应聘人性别
		/// </summary>
		public string gender{ get; set; }
		/// <summary>
		/// 应聘人出生年月
		/// </summary>
		public Nullable<DateTime> birthday{ get; set; }
		/// <summary>
		/// 应聘人联系电话
		/// </summary>
		public string phone{ get; set; }
		/// <summary>
		/// 婚姻状况
		/// </summary>
		public string marriage{ get; set; }
		/// <summary>
		/// 学历
		/// </summary>
		public string education{ get; set; }
		/// <summary>
		/// 微信名
		/// </summary>
		public string wechat_username{ get; set; }
		/// <summary>
		/// 微信号
		/// </summary>
		public string wechat_user_id{ get; set; }
		/// <summary>
		/// 现住址
		/// </summary>
		public string address{ get; set; }
		/// <summary>
		/// 家庭成员姓名
		/// </summary>
		public string family_username{ get; set; }
		/// <summary>
		/// 与家庭成员关系
		/// </summary>
		public string family_relationship{ get; set; }
		/// <summary>
		/// 家庭成员年龄
		/// </summary>
		public string family_age{ get; set; }
		/// <summary>
		/// 家庭成员单位名称
		/// </summary>
		public string family_job_company{ get; set; }
		/// <summary>
		/// 家庭成员职务
		/// </summary>
		public string family_job{ get; set; }
		/// <summary>
		/// 家庭成员联系方式
		/// </summary>
		public string family_phone{ get; set; }
		/// <summary>
		/// 政治面貌
		/// </summary>
		public string feild1{ get; set; }
			}
	    			/// <summary>
			/// 表实体-邀约线下 
			/// </summary>	
			public class p_yaoyuexx : ModelDbBase
			{    
							public p_yaoyuexx(){}
				public p_yaoyuexx(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.qudao_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 邀约日期
		/// </summary>
		public Nullable<DateTime> c_date{ get; set; }
		/// <summary>
		/// 外宣联系人
		/// </summary>
		public string wx_sn{ get; set; }
		/// <summary>
		/// 渠道
		/// </summary>
		public Nullable<int> qudao_id{ get; set; }
		/// <summary>
		/// 微信昵称
		/// </summary>
		public string wx_nick{ get; set; }
		/// <summary>
		/// 微信号
		/// </summary>
		public string wx_account{ get; set; }
		/// <summary>
		/// 性别
		/// </summary>
		public string gender{ get; set; }
		/// <summary>
		/// 预约来面试日期
		/// </summary>
		public Nullable<DateTime> p_date{ get; set; }
		/// <summary>
		/// 实际来面试日期
		/// </summary>
		public Nullable<DateTime> s_date{ get; set; }
		/// <summary>
		/// 面试人sn(外宣端角色为面试)
		/// </summary>
		public string mser_sn{ get; set; }
		/// <summary>
		/// 面试结果#enum:通过=0;不通过=1
		/// </summary>
		public Nullable<sbyte> ms_status{ get; set; }
		public enum ms_status_enum {
			通过=0,
			不通过=1,
		}
		/// <summary>
		/// 预计入职时间
		/// </summary>
		public Nullable<DateTime> p_join_date{ get; set; }
		/// <summary>
		/// 实际入职时间
		/// </summary>
		public Nullable<DateTime> s_join_date{ get; set; }
			}
	    			/// <summary>
			/// 表实体-诊断-每日数据 
			/// </summary>	
			public class p_zhenduan_day : ModelDbBase
			{    
							public p_zhenduan_day(){}
				public p_zhenduan_day(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.exposure_num =  1;
														this.exposure_times =  1;
														this.live_hour =  1m;
														this.join_num =  1;
														this.join_times =  1;
														this.gift_num =  1;
														this.gift_times =  1;
														this.watch_minutes =  1m;
														this.pk_times =  1;
														this.new_fans_num =  1;
														this.amount =  1;
														this.room_click =  1;
														this.ACU =  1m;
														this.comment =  1;
														this.likes =  1;
														this.gift_new =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 直播间标题
		/// </summary>
		public string room_title{ get; set; }
		/// <summary>
		/// 直播开始时间
		/// </summary>
		public Nullable<DateTime> live_start{ get; set; }
		/// <summary>
		/// 直播结束时间
		/// </summary>
		public Nullable<DateTime> live_end{ get; set; }
		/// <summary>
		/// 曝光人数
		/// </summary>
		public Nullable<int> exposure_num{ get; set; }
		/// <summary>
		/// 曝光次数
		/// </summary>
		public Nullable<int> exposure_times{ get; set; }
		/// <summary>
		/// 直播时长
		/// </summary>
		public Nullable<decimal> live_hour{ get; set; }
		/// <summary>
		/// 进直播间人数
		/// </summary>
		public Nullable<int> join_num{ get; set; }
		/// <summary>
		/// 进直播间次数
		/// </summary>
		public Nullable<int> join_times{ get; set; }
		/// <summary>
		/// 打赏人数
		/// </summary>
		public Nullable<int> gift_num{ get; set; }
		/// <summary>
		/// 打赏次数
		/// </summary>
		public Nullable<int> gift_times{ get; set; }
		/// <summary>
		/// 人均观看时长
		/// </summary>
		public Nullable<decimal> watch_minutes{ get; set; }
		/// <summary>
		/// pk次数
		/// </summary>
		public Nullable<int> pk_times{ get; set; }
		/// <summary>
		/// 新增粉丝
		/// </summary>
		public Nullable<int> new_fans_num{ get; set; }
		/// <summary>
		/// 音浪(火力)
		/// </summary>
		public Nullable<int> amount{ get; set; }
		/// <summary>
		/// 直播间封面点击率
		/// </summary>
		public Nullable<int> room_click{ get; set; }
		/// <summary>
		/// 开播端
		/// </summary>
		public string live_platform{ get; set; }
		/// <summary>
		/// 平均在线
		/// </summary>
		public Nullable<decimal> ACU{ get; set; }
		/// <summary>
		/// 评论人数
		/// </summary>
		public Nullable<int> comment{ get; set; }
		/// <summary>
		/// 点赞次数
		/// </summary>
		public Nullable<int> likes{ get; set; }
		/// <summary>
		/// 直播间ip所在地
		/// </summary>
		public string room_ip{ get; set; }
		/// <summary>
		/// 新人打赏人数
		/// </summary>
		public Nullable<int> gift_new{ get; set; }
			}
	    			/// <summary>
			/// 表实体-支付方式配置 
			/// </summary>	
			public class pay_config : ModelDbModularUserBasic.pay_config
			{    
							public pay_config(){}
				public pay_config(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.sort =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-支付方式租户 
			/// </summary>	
			public class pay_tenant : ModelDbModularUserBasic.pay_tenant
			{    
							public pay_tenant(){}
				public pay_tenant(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-数据比较规则 
			/// </summary>	
			public class projectbasic_data_rule : ModelDbBase
			{    
							public projectbasic_data_rule(){}
				public projectbasic_data_rule(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.type_id =  1;
														this.min_value =  1m;
														this.max_value =  1m;
														this.font_size =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属规则类型id
		/// </summary>
		public Nullable<int> type_id{ get; set; }
		/// <summary>
		/// 字段
		/// </summary>
		public string field_key{ get; set; }
		/// <summary>
		/// 字段名称
		/// </summary>
		public string field_name{ get; set; }
		/// <summary>
		/// 最小值
		/// </summary>
		public Nullable<decimal> min_value{ get; set; }
		/// <summary>
		/// 最大值
		/// </summary>
		public Nullable<decimal> max_value{ get; set; }
		/// <summary>
		/// 字体大小
		/// </summary>
		public Nullable<int> font_size{ get; set; }
		/// <summary>
		/// 字体颜色
		/// </summary>
		public string font_color{ get; set; }
		/// <summary>
		/// 规则：字段明:比较范围，比较直，颜色
		/// </summary>
		public string c_rule{ get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string remake{ get; set; }
			}
	    			/// <summary>
			/// 表实体-数据比较规则 
			/// </summary>	
			public class projectbasic_data_rule_type : ModelDbBase
			{    
							public projectbasic_data_rule_type(){}
				public projectbasic_data_rule_type(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 模块名称
		/// </summary>
		public string name{ get; set; }
			}
	    			/// <summary>
			/// 表实体-节奏梳理-问题明细 
			/// </summary>	
			public class qns_questions : ModelDbBase
			{    
							public qns_questions(){}
				public qns_questions(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.step =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属阶段
		/// </summary>
		public Nullable<int> step{ get; set; }
		/// <summary>
		/// 遇到问题
		/// </summary>
		public string question{ get; set; }
		/// <summary>
		/// 问题编号
		/// </summary>
		public string qns_sn{ get; set; }
		/// <summary>
		/// 所属节奏sn
		/// </summary>
		public string jiezou_sn{ get; set; }
		/// <summary>
		/// 运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 排序
		/// </summary>
		public Nullable<int> sort{ get; set; }
			}
	    			/// <summary>
			/// 表实体-节奏梳理-解决方案 
			/// </summary>	
			public class qns_solutions : ModelDbBase
			{    
							public qns_solutions(){}
				public qns_solutions(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 解决方案
		/// </summary>
		public string solution{ get; set; }
		/// <summary>
		/// 问题编号
		/// </summary>
		public string qns_sn{ get; set; }
		/// <summary>
		/// 运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 解决方案编号
		/// </summary>
		public string solution_sn{ get; set; }
		/// <summary>
		/// 排序
		/// </summary>
		public Nullable<int> sort{ get; set; }
			}
	    			/// <summary>
			/// 表实体-门户网站-通用设置 
			/// </summary>	
			public class site_common : ModelDbModularSite.site_common
			{    
							public site_common(){}
				public site_common(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.default_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-门户网站-配置信息 
			/// </summary>	
			public class site_config : ModelDbModularSite.site_config
			{    
							public site_config(){}
				public site_config(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			public enum k_type_enum {
			文字=1,
			图片=2,
		}
			}
	    			/// <summary>
			/// 表实体-门户网站-配置信息-参数 
			/// </summary>	
			public class site_config_para : ModelDbModularSite.site_config_para
			{    
							public site_config_para(){}
				public site_config_para(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.sort =  1;
												}
				}
			public enum k_type_enum {
			文字=1,
			图片=2,
		}
			}
	    			/// <summary>
			/// 表实体-门户网站-内容 
			/// </summary>	
			public class site_content : ModelDbModularSite.site_content
			{    
							public site_content(){}
				public site_content(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			public enum c_type_enum {
			导航菜单=1,
			图文广告=2,
			新闻公告=3,
			产品服务=4,
		}
			}
	    			/// <summary>
			/// 表实体-门户网站-内容-导航菜单 
			/// </summary>	
			public class site_content_nav : ModelDbModularSite.site_content_nav
			{    
							public site_content_nav(){}
				public site_content_nav(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.content_id =  1;
														this.parent_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-门户网站-内容-新闻文章 
			/// </summary>	
			public class site_content_news : ModelDbModularSite.site_news
			{    
							public site_content_news(){}
				public site_content_news(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-门户网站-内容-新闻文章-类别 
			/// </summary>	
			public class site_content_news_cate : ModelDbModularSite.site_news_cate
			{    
							public site_content_news_cate(){}
				public site_content_news_cate(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.parent_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-门户网站-母版页 
			/// </summary>	
			public class site_master : ModelDbModularSite.site_master
			{    
							public site_master(){}
				public site_master(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			public enum m_type_enum {
			文件=1,
			内容=2,
		}
			}
	    			/// <summary>
			/// 表实体-门户网站-页面 
			/// </summary>	
			public class site_page : ModelDbModularSite.site_page
			{    
							public site_page(){}
				public site_page(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.view_auth =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-附加数据 
			/// </summary>	
			public class sys_attchdata : ModelDbBase
			{    
							public sys_attchdata(){}
				public sys_attchdata(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 对应表名
		/// </summary>
		public string tb_name{ get; set; }
			}
	    			/// <summary>
			/// 表实体-附加数据-字段 
			/// </summary>	
			public class sys_attchdata__item : ModelDbBase
			{    
							public sys_attchdata__item(){}
				public sys_attchdata__item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.attchdata_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Nullable<int> attchdata_id{ get; set; }
		/// <summary>
		/// 字段名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 字段类型：文本，下拉框，多行文本
		/// </summary>
		public Nullable<sbyte> f_type{ get; set; }
		/// <summary>
		/// 数据字典名
		/// </summary>
		public string dic_name{ get; set; }
		/// <summary>
		/// 初始值
		/// </summary>
		public string default_value{ get; set; }
		/// <summary>
		/// 排序号，越小越靠前
		/// </summary>
		public Nullable<int> sort{ get; set; }
		/// <summary>
		/// 是否作为筛选字段
		/// </summary>
		public Nullable<sbyte> is_search{ get; set; }
		/// <summary>
		/// 是否作为显示列
		/// </summary>
		public Nullable<sbyte> is_display{ get; set; }
		/// <summary>
		/// 是否为必填项
		/// </summary>
		public Nullable<sbyte> is_required{ get; set; }
			}
	    			/// <summary>
			/// 表实体-系统-业务日志 
			/// </summary>	
			public class sys_biz_log : ModelDbModularBasic.sys_biz_log
			{    
							public sys_biz_log(){}
				public sys_biz_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.log_type =  1;
														this.user_type_id =  1;
												}
				}
			/// <summary>
		/// 租户id#base:tenant_id
		/// </summary>
		public Nullable<int> tenant_id{
		get {
		return base.tenant_id;
		}
		set {
		base.tenant_id = value;
		}
		}
		/// <summary>
		/// 日志类型#enum:用户模块=1;商品模块=2;产品模块=3;订单模块=4;财务模块=5#base:log_type
		/// </summary>
		public Nullable<int> log_type{
		get {
		return base.log_type;
		}
		set {
		base.log_type = value;
		}
		}
		public enum log_type_enum {
			用户模块=1,
			商品模块=2,
			产品模块=3,
			订单模块=4,
			财务模块=5,
		}
		/// <summary>
		/// 模块功能名称#base:modular_function
		/// </summary>
		public string modular_function{
		get {
		return base.modular_function;
		}
		set {
		base.modular_function = value;
		}
		}
		/// <summary>
		/// 关联编号#base:relation_sn
		/// </summary>
		public string relation_sn{
		get {
		return base.relation_sn;
		}
		set {
		base.relation_sn = value;
		}
		}
		/// <summary>
		/// 客户端ip#base:client_ip
		/// </summary>
		public string client_ip{
		get {
		return base.client_ip;
		}
		set {
		base.client_ip = value;
		}
		}
		/// <summary>
		/// 操作人类型#base:user_type_id
		/// </summary>
		public Nullable<int> user_type_id{
		get {
		return base.user_type_id;
		}
		set {
		base.user_type_id = value;
		}
		}
		/// <summary>
		/// 用户编号#base:user_sn
		/// </summary>
		public string user_sn{
		get {
		return base.user_sn;
		}
		set {
		base.user_sn = value;
		}
		}
		/// <summary>
		/// 备注信息#base:memo
		/// </summary>
		public string memo{
		get {
		return base.memo;
		}
		set {
		base.memo = value;
		}
		}
		/// <summary>
		/// 模块测试说明#base:modular_memo
		/// </summary>
		public string modular_memo{
		get {
		return base.modular_memo;
		}
		set {
		base.modular_memo = value;
		}
		}
			}
	    			/// <summary>
			/// 表实体-系统-对象容器 
			/// </summary>	
			public class sys_container : ModelDbBase
			{    
							public sys_container(){}
				public sys_container(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 对象类型：1=当前租户；2=当前用户类型；3=当前租户和用户类型；4=当前登录用户
		/// </summary>
		public Nullable<sbyte> c_type{ get; set; }
		/// <summary>
		/// 对象类型关联sn
		/// </summary>
		public string c_relate_sn{ get; set; }
		/// <summary>
		/// 对象名
		/// </summary>
		public string c_name{ get; set; }
		/// <summary>
		/// 用户为相同类型数据指定不同的key，可以为空
		/// </summary>
		public string c_key{ get; set; }
		/// <summary>
		/// 对象类型值 json格式
		/// </summary>
		public string c_value{ get; set; }
		/// <summary>
		/// 过期时间
		/// </summary>
		public Nullable<DateTime> expire_time{ get; set; }
		/// <summary>
		/// 备注信息
		/// </summary>
		public string memo{ get; set; }
			}
	    			/// <summary>
			/// 表实体-系统-字典 
			/// </summary>	
			public class sys_dict : ModelDbModularBasic.sys_dict
			{    
							public sys_dict(){}
				public sys_dict(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.category_id =  1;
														this.pid =  1;
														this.sort =  1;
												}
				}
			public enum is_abled_enum {
			已关闭=0,
			已开启=1,
		}
			}
	    			/// <summary>
			/// 表实体-系统-字典类别 
			/// </summary>	
			public class sys_dict_category : ModelDbModularBasic.sys_dict_category
			{    
							public sys_dict_category(){}
				public sys_dict_category(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.parent_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-系统-枚举 
			/// </summary>	
			public class sys_enum : ModelDbModularBasic.sys_enum
			{    
							public sys_enum(){}
				public sys_enum(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.sort =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-系统-枚举项 
			/// </summary>	
			public class sys_enum_item : ModelDbModularBasic.sys_enum_item
			{    
							public sys_enum_item(){}
				public sys_enum_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.sort =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-事件-发生的事实 
			/// </summary>	
			public class sys_event_fact : ModelDbBase
			{    
							public sys_event_fact(){}
				public sys_event_fact(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 来源URL
		/// </summary>
		public string from_url{ get; set; }
		/// <summary>
		/// 用户sn
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 事件名
		/// </summary>
		public string event_name{ get; set; }
		/// <summary>
		/// 参数信息
		/// </summary>
		public string para{ get; set; }
		/// <summary>
		/// 推送状态 0=待处理 1=已处理
		/// </summary>
		public Nullable<sbyte> push_status{ get; set; }
		/// <summary>
		/// 推送时间
		/// </summary>
		public Nullable<DateTime> push_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-事件-推送记录 
			/// </summary>	
			public class sys_event_push : ModelDbBase
			{    
							public sys_event_push(){}
				public sys_event_push(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.fact_id =  1;
														this.push_times =  1;
												}
				}
			/// <summary>
		/// 推送唯一编号
		/// </summary>
		public string push_sn{ get; set; }
		/// <summary>
		/// 事件事实id
		/// </summary>
		public Nullable<int> fact_id{ get; set; }
		/// <summary>
		/// 事件名称
		/// </summary>
		public string event_name{ get; set; }
		/// <summary>
		/// 事件订阅url
		/// </summary>
		public string url{ get; set; }
		/// <summary>
		/// 提交参数
		/// </summary>
		public string para{ get; set; }
		/// <summary>
		/// 返回状态1=成功;2=失败
		/// </summary>
		public Nullable<sbyte> push_status{ get; set; }
		/// <summary>
		/// 最后返回结果
		/// </summary>
		public string push_result{ get; set; }
		/// <summary>
		/// 最后推送时间
		/// </summary>
		public Nullable<DateTime> push_time{ get; set; }
		/// <summary>
		/// 累计推送次数
		/// </summary>
		public Nullable<int> push_times{ get; set; }
			}
	    			/// <summary>
			/// 表实体-事件-订阅的URL 
			/// </summary>	
			public class sys_event_url : ModelDbBase
			{    
							public sys_event_url(){}
				public sys_event_url(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.modular_id =  1;
														this.t_modular_id =  1;
														this.need_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 模块id
		/// </summary>
		public Nullable<int> modular_id{ get; set; }
		/// <summary>
		/// 模块名称
		/// </summary>
		public string modular_name{ get; set; }
		/// <summary>
		/// 订阅方的模块id
		/// </summary>
		public Nullable<int> t_modular_id{ get; set; }
		/// <summary>
		/// 订阅方的模块名称
		/// </summary>
		public string t_modular_name{ get; set; }
		/// <summary>
		/// 按需功能id
		/// </summary>
		public Nullable<int> need_id{ get; set; }
		/// <summary>
		/// 模块名-服务名事件名
		/// </summary>
		public string event_name{ get; set; }
		/// <summary>
		/// 订阅URL
		/// </summary>
		public string url{ get; set; }
		/// <summary>
		/// 备注信息
		/// </summary>
		public string memo{ get; set; }
		/// <summary>
		/// 状态#enum:正常=0;停用=1
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			正常=0,
			停用=1,
		}
			}
	    			/// <summary>
			/// 表实体-系统-模块 
			/// </summary>	
			public class sys_modular : ModelDbModularBasic.sys_modular
			{    
							public sys_modular(){}
				public sys_modular(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-系统-模块-角色功能 
			/// </summary>	
			public class sys_modular_function : ModelDbModularBasic.sys_modular_function
			{    
							public sys_modular_function(){}
				public sys_modular_function(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.user_type_id =  1;
														this.parent_id =  1;
														this.sort =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-系统-模块-平台远程赋值，功能配置值（1.功能开关） 
			/// </summary>	
			public class sys_modular_functionconfig : ModelDbModularBasic.sys_modular_functionconfig
			{    
							public sys_modular_functionconfig(){}
				public sys_modular_functionconfig(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
												}
				}
			public enum f_type_enum {
			按需功能=1,
			功能配置=2,
			页面元素=3,
		}
			}
	    			/// <summary>
			/// 表实体-系统-模块-角色菜单 
			/// </summary>	
			public class sys_modular_menu : ModelDbModularBasic.sys_modular_menu
			{    
							public sys_modular_menu(){}
				public sys_modular_menu(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.modular_id =  1;
														this.user_type_id =  1;
														this.parent_id =  1;
														this.sort =  1;
												}
				}
			public enum is_open_enum {
			关闭=0,
			展开=1,
		}
		public enum n_type_enum {
			树形=1,
			附属=2,
			标签=3,
		}
			}
	    			/// <summary>
			/// 表实体-系统-模块-平台远程赋值，流程按需引用 
			/// </summary>	
			public class sys_modular_needconfig : ModelDbModularBasic.sys_modular_needconfig
			{    
							public sys_modular_needconfig(){}
				public sys_modular_needconfig(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-模块-视图页面-配置值(1.功能级参数值，2.页面级参数值，3.视图页面模板) 
			/// </summary>	
			public class sys_modular_pageconfig : ModelDbModularBasic.sys_modular_pageconfig
			{    
							public sys_modular_pageconfig(){}
				public sys_modular_pageconfig(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
												}
				}
			public enum f_type_enum {
			功能级参数值=1,
			页面级参数值=2,
			视图页面模板=3,
		}
			}
	    			/// <summary>
			/// 表实体-系统-消息 
			/// </summary>	
			public class sys_notice : ModelDbModularBasic.sys_notice
			{    
							public sys_notice(){}
				public sys_notice(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.category_id =  1;
												}
				}
			public enum is_read_enum {
			未读=0,
			已读=1,
		}
			}
	    			/// <summary>
			/// 表实体-系统-消息类别 
			/// </summary>	
			public class sys_notice_category : ModelDbModularBasic.sys_notice_category
			{    
							public sys_notice_category(){}
				public sys_notice_category(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.user_type_id =  1;
														this.tenant_id =  1;
												}
				}
			public enum s_type_enum {
			系统=0,
			用户=1,
		}
		public enum sys_code_enum {
			生日=0,
			离职=1,
			转正=2,
			合同到期=3,
			
				/// 资产归还=4,
			公司公告=5,
		}
			}
	    			/// <summary>
			/// 表实体-系统-组织结构 
			/// </summary>	
			public class sys_organize : ModelDbModularBasic.sys_organize
			{    
							public sys_organize(){}
				public sys_organize(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
														this.parent_id =  1;
														this.sort =  1;
												}
				}
			public enum is_default_enum {
			否=0,
			是=1,
		}
			}
	    			/// <summary>
			/// 表实体-系统-职位 
			/// </summary>	
			public class sys_position : ModelDbModularBasic.sys_position
			{    
							public sys_position(){}
				public sys_position(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
														this.parent_id =  1;
														this.sort =  1;
												}
				}
			public enum is_default_enum {
			否=0,
			是=1,
		}
			}
	    			/// <summary>
			/// 表实体-系统-能力模板-实例 
			/// </summary>	
			public class sys_power_newer : ModelDbModularBasic.sys_power_newer
			{    
							public sys_power_newer(){}
				public sys_power_newer(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-系统-角色 
			/// </summary>	
			public class sys_role : ModelDbModularBasic.sys_role
			{    
							public sys_role(){}
				public sys_role(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
														this.workbench_id =  1;
												}
				}
			public enum data_view_enum {
			仅自己=0,
			所在部门=1,
			全部=2,
		}
		public enum is_default_enum {
			否=0,
			是=1,
		}
			}
	    			/// <summary>
			/// 表实体- 
			/// </summary>	
			public class sys_role__extra : ModelDbBase
			{    
							public sys_role__extra(){}
				public sys_role__extra(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
														this.data_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 用户端类型id
		/// </summary>
		public Nullable<int> user_type_id{ get; set; }
		/// <summary>
		/// 限制类型：1=禁止；2=允许
		/// </summary>
		public Nullable<sbyte> b_type{ get; set; }
		/// <summary>
		/// 权限类型：1=菜单；2=功能
		/// </summary>
		public Nullable<sbyte> c_type{ get; set; }
		/// <summary>
		/// 菜单或功能id
		/// </summary>
		public Nullable<int> data_id{ get; set; }
			}
	    			/// <summary>
			/// 表实体-系统-角色功能关联 
			/// </summary>	
			public class sys_role__function : ModelDbModularBasic.sys_role__function
			{    
							public sys_role__function(){}
				public sys_role__function(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.role_id =  1;
														this.function_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-系统-角色菜单关联 
			/// </summary>	
			public class sys_role__menu : ModelDbModularBasic.sys_role__menu
			{    
							public sys_role__menu(){}
				public sys_role__menu(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.role_id =  1;
														this.menu_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-系统-短网址 
			/// </summary>	
			public class sys_short_url : ModelDbModularBasic.sys_short_url
			{    
							public sys_short_url(){}
				public sys_short_url(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-系统-租户 
			/// </summary>	
			public class sys_tenant : ModelDbModularBasic.sys_tenant
			{    
							public sys_tenant(){}
				public sys_tenant(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-系统-租户-域名 
			/// </summary>	
			public class sys_tenant_domain : ModelDbModularBasic.sys_tenant_domain
			{    
							public sys_tenant_domain(){}
				public sys_tenant_domain(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
														this.support_weixinsiteid =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-系统-租户-配置方案 
			/// </summary>	
			public class sys_tenant_plan : ModelDbModularBasic.sys_tenant_plan
			{    
							public sys_tenant_plan(){}
				public sys_tenant_plan(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.user_type_id =  1;
														this.level =  1;
												}
				}
			/// <summary>
		/// 项目字段#project:
		/// </summary>
		public string project_feild{ get; set; }
			}
	    			/// <summary>
			/// 表实体-系统-数据修改版本 
			/// </summary>	
			public class sys_update_version : ModelDbModularBasic.sys_update_version
			{    
							public sys_update_version(){}
				public sys_update_version(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.data_id =  1;
														this.user_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-工作台 
			/// </summary>	
			public class sys_workbench : ModelDbModularBasic.sys_workbench
			{    
							public sys_workbench(){}
				public sys_workbench(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.user_type_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-工作台的组件 
			/// </summary>	
			public class sys_workbench_item : ModelDbModularBasic.sys_workbench_item
			{    
							public sys_workbench_item(){}
				public sys_workbench_item(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.workbench_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体- 
			/// </summary>	
			public class tingguan_test : ModelDbBase
			{    
							public tingguan_test(){}
				public tingguan_test(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
												}
				}
			/// <summary>
		/// 登录账号
		/// </summary>
		public string tg_username{ get; set; }
		/// <summary>
		/// 所属运营
		/// </summary>
		public string yy_name{ get; set; }
		/// <summary>
		/// 上级厅管
		/// </summary>
		public string tg_name{ get; set; }
		/// <summary>
		/// 厅管名字
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string attach3{ get; set; }
			}
	    			/// <summary>
			/// 表实体-用户设置 
			/// </summary>	
			public class user__workbench : ModelDbModularBasic.user__workbench
			{    
							public user__workbench(){}
				public user__workbench(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.workbench_id =  1;
														this.item_id =  1;
														this.sort =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-用户 
			/// </summary>	
			public class user_base : ModelDbModularBasic.user_base
			{    
							public user_base(){}
				public user_base(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.organize_id =  1;
														this.user_type_id =  1;
														this.grade_id =  1;
														this.cash_balance =  1m;
														this.trade_balance =  1m;
														this.integral_balance =  1;
														this.integral_total =  1;
														this.login_err_times =  1;
												}
				}
			public enum organize_wt_enum {
			员工=1,
			主管=2,
		}
		public enum status_enum {
			正常=0,
			禁用=1,
			逻辑删除=9,
		}
			}
	    			/// <summary>
			/// 表实体- 
			/// </summary>	
			public class user_info_promotion_zhubo_apply : ModelDbBase
			{    
							public user_info_promotion_zhubo_apply(){}
				public user_info_promotion_zhubo_apply(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.status =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 主播sn
		/// </summary>
		public string user_info_zb_sn{ get; set; }
		/// <summary>
		/// 申请单编号(唯一标识)
		/// </summary>
		public string apply_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 抖音uid
		/// </summary>
		public string dou_uid{ get; set; }
		/// <summary>
		/// 抖音大头号
		/// </summary>
		public string dou_user{ get; set; }
		/// <summary>
		/// 经纪人名字
		/// </summary>
		public string jjr_name{ get; set; }
		/// <summary>
		/// 经纪人uid
		/// </summary>
		public string jjr_uid{ get; set; }
		/// <summary>
		/// 用户名
		/// </summary>
		public string username{ get; set; }
		/// <summary>
		/// 密码
		/// </summary>
		public string password{ get; set; }
		/// <summary>
		/// 手机号码
		/// </summary>
		public string mobile{ get; set; }
		/// <summary>
		/// 申请原因
		/// </summary>
		public string apply_cause{ get; set; }
		/// <summary>
		/// 审批状态#enum:等待运营审批=0;同意=1;拒绝=2
		/// </summary>
		public Nullable<int> status{ get; set; }
		public enum status_enum {
			等待运营审批=0,
			同意=1,
			拒绝=2,
		}
		/// <summary>
		/// 审批时间
		/// </summary>
		public Nullable<DateTime> apply_time{ get; set; }
			}
	    			/// <summary>
			/// 表实体-厅下主播每日人数统计表 
			/// </summary>	
			public class user_info_property_tj : ModelDbBase
			{    
							public user_info_property_tj(){}
				public user_info_property_tj(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.count_a =  1;
														this.count_b =  1;
														this.count_c =  1;
														this.count_full =  1;
														this.count_part =  1;
														this.zb_sum =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 数据时间
		/// </summary>
		public Nullable<DateTime> data_time{ get; set; }
		/// <summary>
		/// A类主播人数
		/// </summary>
		public Nullable<int> count_a{ get; set; }
		/// <summary>
		/// B类主播人数
		/// </summary>
		public Nullable<int> count_b{ get; set; }
		/// <summary>
		/// C类主播人数
		/// </summary>
		public Nullable<int> count_c{ get; set; }
		/// <summary>
		/// 全职人数
		/// </summary>
		public Nullable<int> count_full{ get; set; }
		/// <summary>
		/// 兼职人数
		/// </summary>
		public Nullable<int> count_part{ get; set; }
		/// <summary>
		/// 厅下主播人数
		/// </summary>
		public Nullable<int> zb_sum{ get; set; }
			}
	    			/// <summary>
			/// 表实体-基础数据_直播厅信息 
			/// </summary>	
			public class user_info_tg : ModelDbBase
			{    
							public user_info_tg(){}
				public user_info_tg(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.step =  1m;
														this.join_rate =  1m;
														this.stay_rate =  1m;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属中台sn，没有时为空
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 直播厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 厅名
		/// </summary>
		public string ting_name{ get; set; }
		/// <summary>
		/// 训练厅所属主厅（为空表示主厅）
		/// </summary>
		public string p_ting_sn{ get; set; }
		/// <summary>
		/// 经纪人名字（训练厅为空）
		/// </summary>
		public string jjr_name{ get; set; }
		/// <summary>
		/// 经纪人uid
		/// </summary>
		public string jjr_uid{ get; set; }
		/// <summary>
		/// 厅管头像
		/// </summary>
		public string img_url{ get; set; }
		/// <summary>
		/// 模式
		/// </summary>
		public string mode{ get; set; }
		/// <summary>
		/// 男女厅
		/// </summary>
		public string tg_sex{ get; set; }
		/// <summary>
		/// 管理微信号
		/// </summary>
		public string manager_wx{ get; set; }
		/// <summary>
		/// 抖音大头号
		/// </summary>
		public string dou_user{ get; set; }
		/// <summary>
		/// 抖音大头号1
		/// </summary>
		public string dou_user1{ get; set; }
		/// <summary>
		/// 抖音大头号2
		/// </summary>
		public string dou_user2{ get; set; }
		/// <summary>
		/// 抖音UID
		/// </summary>
		public string dou_UID{ get; set; }
		/// <summary>
		/// 抖音UID1
		/// </summary>
		public string dou_UID1{ get; set; }
		/// <summary>
		/// 抖音UID2
		/// </summary>
		public string dou_UID2{ get; set; }
		/// <summary>
		/// 电话
		/// </summary>
		public string phone{ get; set; }
		/// <summary>
		/// 生日
		/// </summary>
		public Nullable<DateTime> birthday{ get; set; }
		/// <summary>
		/// mbti
		/// </summary>
		public string mbti{ get; set; }
		/// <summary>
		/// 开厅时间
		/// </summary>
		public TimeSpan open_ting_time{ get; set; }
		/// <summary>
		/// 目前在开档
		/// </summary>
		public string current_open_dangwei{ get; set; }
		/// <summary>
		/// 加入公会时间
		/// </summary>
		public Nullable<DateTime> join_party_time{ get; set; }
		/// <summary>
		/// 地址
		/// </summary>
		public string address{ get; set; }
		/// <summary>
		/// 当前阶段#enum:第一阶段=1;第二阶段=2;第三阶段=3;第四阶段=4;第五阶段=5
		/// </summary>
		public Nullable<decimal> step{ get; set; }
		public enum step_enum {
			第一阶段=1,
			第二阶段=2,
			第三阶段=3,
			第四阶段=4,
			第五阶段=5,
		}
		/// <summary>
		/// 补人率
		/// </summary>
		public Nullable<decimal> join_rate{ get; set; }
		/// <summary>
		/// 留人率
		/// </summary>
		public Nullable<decimal> stay_rate{ get; set; }
		/// <summary>
		/// 状态#enum:正常=0;禁用=1;逻辑删除=9
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			正常=0,
			禁用=1,
			逻辑删除=9,
		}
			}
	    			/// <summary>
			/// 表实体-用户信息-厅-开厅申请 
			/// </summary>	
			public class user_info_ting_apply : ModelDbBase
			{    
							public user_info_ting_apply(){}
				public user_info_ting_apply(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 申请单编号(唯一标识)
		/// </summary>
		public string apply_sn{ get; set; }
		/// <summary>
		/// 运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 原接档厅
		/// </summary>
		public string old_ting_name{ get; set; }
		/// <summary>
		/// 主厅sn
		/// </summary>
		public string p_ting_sn{ get; set; }
		/// <summary>
		/// 抖音账号
		/// </summary>
		public string dy_account{ get; set; }
		/// <summary>
		/// 真实姓名
		/// </summary>
		public string real_name{ get; set; }
		/// <summary>
		/// 手机后四位
		/// </summary>
		public string moblie_lastfour{ get; set; }
		/// <summary>
		/// 经纪人名字
		/// </summary>
		public string jjr_name{ get; set; }
		/// <summary>
		/// 状态#enum:等待超管审批=0;等待开通=1;已完成=3;已取消=4;已拒绝=9
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			等待超管审批=0,
			等待开通=1,
			已完成=3,
			已取消=4,
			已拒绝=9,
		}
			}
	    			/// <summary>
			/// 表实体-用户信息-厅-日志 
			/// </summary>	
			public class user_info_ting_log : ModelDbBase
			{    
							public user_info_ting_log(){}
				public user_info_ting_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 操作类型#enum:开厅=1;关厅=2;绑定大头号=3;解绑大头号=4
		/// </summary>
		public Nullable<sbyte> c_type{ get; set; }
		public enum c_type_enum {
			开厅=1,
			关厅=2,
			绑定大头号=3,
			解绑大头号=4,
		}
		/// <summary>
		/// 关联user_info_tg的ting_sn
		/// </summary>
		public string user_info_ting_sn{ get; set; }
		/// <summary>
		/// 操作时厅状态#enum:正常=1;已关闭=2
		/// </summary>
		public Nullable<sbyte> ting_status{ get; set; }
		public enum ting_status_enum {
			正常=1,
			已关闭=2,
		}
		/// <summary>
		/// 内容描述
		/// </summary>
		public string content{ get; set; }
		/// <summary>
		/// 操作人用户类型id
		/// </summary>
		public Nullable<int> user_type_id{ get; set; }
		/// <summary>
		/// 操作人
		/// </summary>
		public string user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-基础数据_厅管信息 
			/// </summary>	
			public class user_info_tingguan : ModelDbBase
			{    
							public user_info_tingguan(){}
				public user_info_tingguan(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属中台sn，没有时为空
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属厅管用户编号
		/// </summary>
		public string tg_user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-基础数据_运营信息 
			/// </summary>	
			public class user_info_yunying : ModelDbBase
			{    
							public user_info_yunying(){}
				public user_info_yunying(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.zt_organize_id =  1;
														this.join_pintotop_times =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属中台组织id
		/// </summary>
		public Nullable<int> zt_organize_id{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 补人置顶次数（每月默认5次）
		/// </summary>
		public Nullable<int> join_pintotop_times{ get; set; }
			}
	    			/// <summary>
			/// 表实体-用户信息-运营-新人判定规则设置 
			/// </summary>	
			public class user_info_yy_newer : ModelDbBase
			{    
							public user_info_yy_newer(){}
				public user_info_yy_newer(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.in_days =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 创建账号几天内算新人，默认值为5
		/// </summary>
		public Nullable<int> in_days{ get; set; }
			}
	    			/// <summary>
			/// 表实体-基础信息-运营-运营权限 
			/// </summary>	
			public class user_info_yy_power : ModelDbBase
			{    
							public user_info_yy_power(){}
				public user_info_yy_power(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 可查看模块名称，多个用逗号隔开(主播账号)
		/// </summary>
		public string able_names{ get; set; }
		/// <summary>
		/// 状态#enum:正常=0;禁用=1;逻辑删除=9
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			正常=0,
			禁用=1,
			逻辑删除=9,
		}
			}
	    			/// <summary>
			/// 表实体-基础信息-运营-关联团队 
			/// </summary>	
			public class user_info_yy_with : ModelDbBase
			{    
							public user_info_yy_with(){}
				public user_info_yy_with(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属运营用户编号
		/// </summary>
		public string f_yy_user_sn{ get; set; }
		/// <summary>
		/// 被关联的运营用户编号，多个用逗号隔开
		/// </summary>
		public string t_yy_user_sns{ get; set; }
		/// <summary>
		/// 可查看模块名称，多个用逗号隔开(数据表格，运营4.0)
		/// </summary>
		public string able_names{ get; set; }
		/// <summary>
		/// 状态#enum:正常=0;禁用=1;逻辑删除=9
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			正常=0,
			禁用=1,
			逻辑删除=9,
		}
			}
	    			/// <summary>
			/// 表实体-用户-信息-主播信息 
			/// </summary>	
			public class user_info_zb : ModelDbBase
			{    
							public user_info_zb(){}
				public user_info_zb(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.tg_need_id =  1;
														this.tg_dangwei =  1;
														this.age =  1;
														this.devices_num =  1;
														this.supplement_sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 唯一编号
		/// </summary>
		public string user_info_zb_sn{ get; set; }
		/// <summary>
		/// 所属主播sn(成为主播前为空)
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 对接厅管（所属厅管user_sn） 2
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属中台sn
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 上一任对接厅管
		/// </summary>
		public string old_tg_user_sn{ get; set; }
		/// <summary>
		/// 上一任对接厅厅名
		/// </summary>
		public string old_tg_username{ get; set; }
		/// <summary>
		/// 对应补人申请表id
		/// </summary>
		public Nullable<int> tg_need_id{ get; set; }
		/// <summary>
		/// 对应补人申请表档位id
		/// </summary>
		public Nullable<int> tg_dangwei{ get; set; }
		/// <summary>
		/// 签约sn 1
		/// </summary>
		public string qy_sn{ get; set; }
		/// <summary>
		/// 所属萌新sn
		/// </summary>
		public string mx_sn{ get; set; }
		/// <summary>
		/// 所属流量sn
		/// </summary>
		public string wx_sn{ get; set; }
		/// <summary>
		/// 主播名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 直播账号id(删)
		/// </summary>
		public string app_id{ get; set; }
		/// <summary>
		/// 年龄 3
		/// </summary>
		public Nullable<int> age{ get; set; }
		/// <summary>
		/// 是否已婚 3
		/// </summary>
		public string marriage{ get; set; }
		/// <summary>
		/// 入职日期
		/// </summary>
		public Nullable<DateTime> join_date{ get; set; }
		/// <summary>
		/// 离职日期
		/// </summary>
		public Nullable<DateTime> leave_date{ get; set; }
		/// <summary>
		/// 有无孩子 3
		/// </summary>
		public string child{ get; set; }
		/// <summary>
		/// 声卡 3
		/// </summary>
		public string sound_card{ get; set; }
		/// <summary>
		/// 兼职全职 123
		/// </summary>
		public string full_or_part{ get; set; }
		/// <summary>
		/// 地区(省市) 123
		/// </summary>
		public string address{ get; set; }
		/// <summary>
		/// 所在省份
		/// </summary>
		public string province{ get; set; }
		/// <summary>
		/// 所在城市
		/// </summary>
		public string city{ get; set; }
		/// <summary>
		/// 直播经验 3
		/// </summary>
		public string experience{ get; set; }
		/// <summary>
		/// 现实工作 23
		/// </summary>
		public string job{ get; set; }
		/// <summary>
		/// 目标收入 3
		/// </summary>
		public string revenue{ get; set; }
		/// <summary>
		/// 接档时间（字典：档位时段id,多个用逗号隔开） 123
		/// </summary>
		public string sessions{ get; set; }
		/// <summary>
		/// 生日 3
		/// </summary>
		public string birthday{ get; set; }
		/// <summary>
		/// 星座 3
		/// </summary>
		public string star_sign{ get; set; }
		/// <summary>
		/// 才艺 3
		/// </summary>
		public string talent{ get; set; }
		/// <summary>
		/// 招聘渠道 3
		/// </summary>
		public string way{ get; set; }
		/// <summary>
		/// 电话号码 3
		/// </summary>
		public string mobile{ get; set; }
		/// <summary>
		/// 设备数量 3
		/// </summary>
		public Nullable<int> devices_num{ get; set; }
		/// <summary>
		/// 学历(字典：学历) 3
		/// </summary>
		public Nullable<sbyte> education{ get; set; }
		/// <summary>
		/// mbti人格 3
		/// </summary>
		public string mbti{ get; set; }
		/// <summary>
		/// 是否负债 3
		/// </summary>
		public string is_low{ get; set; }
		/// <summary>
		/// 微信id
		/// </summary>
		public string wechat_id{ get; set; }
		/// <summary>
		/// 微信昵称 12
		/// </summary>
		public string wechat_nickname{ get; set; }
		/// <summary>
		/// 微信账号2
		/// </summary>
		public string wechat_username{ get; set; }
		/// <summary>
		/// 抖音账号 12
		/// </summary>
		public string dou_username{ get; set; }
		/// <summary>
		/// 抖音昵称 2
		/// </summary>
		public string dou_nickname{ get; set; }
		/// <summary>
		/// 性别 12
		/// </summary>
		public string zb_sex{ get; set; }
		/// <summary>
		/// 对接群 1
		/// </summary>
		public string qun{ get; set; }
		/// <summary>
		/// 是否进群 1
		/// </summary>
		public string qun_in{ get; set; }
		/// <summary>
		/// 说明
		/// </summary>
		public string note{ get; set; }
		/// <summary>
		/// 主播质量
		/// </summary>
		public Nullable<sbyte> quality{ get; set; }
		/// <summary>
		/// 微信openid
		/// </summary>
		public string openid{ get; set; }
		/// <summary>
		/// 推人状态#enum:等待添加签约人=0;已添加签约人=1;已作废=2;不合格=3;合格=4
		/// </summary>
		public Nullable<sbyte> tui_status{ get; set; }
		public enum tui_status_enum {
			等待添加签约人=0,
			已添加签约人=1,
			已作废=2,
			不合格=3,
			合格=4,
		}
		/// <summary>
		/// 期数
		/// </summary>
		public string term{ get; set; }
		/// <summary>
		/// 主播职务
		/// </summary>
		public string position{ get; set; }
		/// <summary>
		/// 主播分类(根据主播的能力职务划分的等级A=厅管、副管、师傅、组长、A类主播；B级=B类主播；C=C类主播)
		/// </summary>
		public string level{ get; set; }
		/// <summary>
		/// 主播分级(萌新端在主播入职时划分的等级)
		/// </summary>
		public string zb_level{ get; set; }
		/// <summary>
		/// 主播分级时间
		/// </summary>
		public Nullable<DateTime> zb_level_time{ get; set; }
		/// <summary>
		/// 分配状态#enum:未分配=0;已分配=1
		/// </summary>
		public Nullable<sbyte> supplement_status{ get; set; }
		public enum supplement_status_enum {
			未分配=0,
			已分配=1,
		}
		/// <summary>
		/// 分配时间（外宣）
		/// </summary>
		public Nullable<DateTime> supplement_time{ get; set; }
		/// <summary>
		/// 长期未分#enum:否=0;是=1
		/// </summary>
		public Nullable<sbyte> longtime{ get; set; }
		public enum longtime_enum {
			否=0,
			是=1,
		}
		/// <summary>
		/// 入库时间
		/// </summary>
		public Nullable<DateTime> put_time{ get; set; }
		/// <summary>
		/// 萌新是否已拉群#enum:未拉群=0;已拉群=1
		/// </summary>
		public Nullable<sbyte> is_qun{ get; set; }
		public enum is_qun_enum {
			未拉群=0,
			已拉群=1,
		}
		/// <summary>
		/// 操作拉群的时间
		/// </summary>
		public Nullable<DateTime> qun_time{ get; set; }
		/// <summary>
		/// 流失时间
		/// </summary>
		public Nullable<DateTime> no_share_time{ get; set; }
		/// <summary>
		/// 流失原因（萌新操作）
		/// </summary>
		public string no_share{ get; set; }
		/// <summary>
		/// 分配优先级
		/// </summary>
		public Nullable<int> supplement_sort{ get; set; }
		/// <summary>
		/// 退回状态#enum:未退回=0;等待萌新处理=1;重新分配=2;流失=3
		/// </summary>
		public Nullable<sbyte> quit_status{ get; set; }
		public enum quit_status_enum {
			未退回=0,
			等待萌新处理=1,
			重新分配=2,
			流失=3,
		}
		/// <summary>
		/// 状态#enum:正常=0;已流失=1;逻辑删除=9
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			正常=0,
			已流失=1,
			逻辑删除=9,
		}
		/// <summary>
		/// 是否加急#enum:不加急=0;加急=1
		/// </summary>
		public Nullable<sbyte> is_fast{ get; set; }
		public enum is_fast_enum {
			不加急=0,
			加急=1,
		}
		/// <summary>
		/// 是否换厅#enum:不换=0;换厅=1
		/// </summary>
		public Nullable<sbyte> is_change{ get; set; }
		public enum is_change_enum {
			不换=0,
			换厅=1,
		}
		/// <summary>
		/// 主播胜任麦 R:红色三麦，G:绿色一麦，O:橙色二麦(废弃20250530)
		/// </summary>
		public string zbsr_color{ get; set; }
		/// <summary>
		/// 抖音作者id（抖音官方主播唯一身份id）
		/// </summary>
		public string anchor_id{ get; set; }
			}
	    			/// <summary>
			/// 表实体-主播属性枚举 
			/// </summary>	
			public class user_info_zb_property1 : ModelDbBase
			{    
							public user_info_zb_property1(){}
				public user_info_zb_property1(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
												}
				}
			/// <summary>
		/// 属性值
		/// </summary>
		public string value{ get; set; }
		/// <summary>
		/// 属性类别
		/// </summary>
		public string key{ get; set; }
			}
	    			/// <summary>
			/// 表实体-用户-信息-主播信息 
			/// </summary>	
			public class user_info_zhubo : ModelDbBase
			{    
							public user_info_zhubo(){}
				public user_info_zhubo(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.tg_need_id =  1;
														this.tg_dangwei =  1;
														this.age =  1;
														this.devices_num =  1;
														this.supplement_sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 唯一编号
		/// </summary>
		public string user_info_zb_sn{ get; set; }
		/// <summary>
		/// 所属主播sn(成为主播前为空)
		/// </summary>
		public string user_sn{ get; set; }
		/// <summary>
		/// 主播昵称(成为主播前为空，作为主播对外显示内容)
		/// </summary>
		public string user_name{ get; set; }
		/// <summary>
		/// 所属厅sn
		/// </summary>
		public string ting_sn{ get; set; }
		/// <summary>
		/// 对接厅管（所属厅管user_sn） 2
		/// </summary>
		public string tg_user_sn{ get; set; }
		/// <summary>
		/// 所属运营sn
		/// </summary>
		public string yy_user_sn{ get; set; }
		/// <summary>
		/// 所属中台sn
		/// </summary>
		public string zt_user_sn{ get; set; }
		/// <summary>
		/// 签约sn 1
		/// </summary>
		public string qy_sn{ get; set; }
		/// <summary>
		/// 所属萌新sn
		/// </summary>
		public string mx_sn{ get; set; }
		/// <summary>
		/// 所属流量sn
		/// </summary>
		public string wx_sn{ get; set; }
		/// <summary>
		/// 上一任对接厅管
		/// </summary>
		public string old_tg_user_sn{ get; set; }
		/// <summary>
		/// 上一任对接厅厅名
		/// </summary>
		public string old_tg_username{ get; set; }
		/// <summary>
		/// 对应补人申请表id
		/// </summary>
		public Nullable<int> tg_need_id{ get; set; }
		/// <summary>
		/// 对应补人申请表档位id
		/// </summary>
		public Nullable<int> tg_dangwei{ get; set; }
		/// <summary>
		/// 主播头像
		/// </summary>
		public string img_url{ get; set; }
		/// <summary>
		/// 主播类型#enum:线上=1;线下=2
		/// </summary>
		public Nullable<sbyte> o_type{ get; set; }
		public enum o_type_enum {
			线上=1,
			线下=2,
		}
		/// <summary>
		/// 年龄 3
		/// </summary>
		public Nullable<int> age{ get; set; }
		/// <summary>
		/// 是否已婚 3
		/// </summary>
		public string marriage{ get; set; }
		/// <summary>
		/// 有无孩子 3
		/// </summary>
		public string child{ get; set; }
		/// <summary>
		/// 生日 3
		/// </summary>
		public string birthday{ get; set; }
		/// <summary>
		/// 星座 3
		/// </summary>
		public string star_sign{ get; set; }
		/// <summary>
		/// 才艺 3
		/// </summary>
		public string talent{ get; set; }
		/// <summary>
		/// 声卡 3
		/// </summary>
		public string sound_card{ get; set; }
		/// <summary>
		/// 招聘渠道 3
		/// </summary>
		public string way{ get; set; }
		/// <summary>
		/// 电话号码 3
		/// </summary>
		public string mobile{ get; set; }
		/// <summary>
		/// 设备数量 3
		/// </summary>
		public Nullable<int> devices_num{ get; set; }
		/// <summary>
		/// 学历(字典：学历) 3
		/// </summary>
		public Nullable<sbyte> education{ get; set; }
		/// <summary>
		/// mbti人格 3
		/// </summary>
		public string mbti{ get; set; }
		/// <summary>
		/// 是否负债 3
		/// </summary>
		public string is_low{ get; set; }
		/// <summary>
		/// 地区(省市) 123
		/// </summary>
		public string address{ get; set; }
		/// <summary>
		/// 所在省份
		/// </summary>
		public string province{ get; set; }
		/// <summary>
		/// 所在城市
		/// </summary>
		public string city{ get; set; }
		/// <summary>
		/// 直播经验 3
		/// </summary>
		public string experience{ get; set; }
		/// <summary>
		/// 现实工作 23
		/// </summary>
		public string job{ get; set; }
		/// <summary>
		/// 目标收入 3
		/// </summary>
		public string revenue{ get; set; }
		/// <summary>
		/// 性别 12
		/// </summary>
		public string zb_sex{ get; set; }
		/// <summary>
		/// 微信openid
		/// </summary>
		public string openid{ get; set; }
		/// <summary>
		/// 微信昵称 12
		/// </summary>
		public string wechat_nickname{ get; set; }
		/// <summary>
		/// 微信账号2
		/// </summary>
		public string wechat_username{ get; set; }
		/// <summary>
		/// 抖音账号 12
		/// </summary>
		public string dou_username{ get; set; }
		/// <summary>
		/// 抖音昵称 2
		/// </summary>
		public string dou_nickname{ get; set; }
		/// <summary>
		/// 抖音作者id（抖音官方主播唯一身份id）
		/// </summary>
		public string anchor_id{ get; set; }
		/// <summary>
		/// 接档时间（字典：档位时段id,多个用逗号隔开） 123
		/// </summary>
		public string sessions{ get; set; }
		/// <summary>
		/// 兼职全职 123
		/// </summary>
		public string full_or_part{ get; set; }
		/// <summary>
		/// 入职日期
		/// </summary>
		public Nullable<DateTime> join_date{ get; set; }
		/// <summary>
		/// 离职日期
		/// </summary>
		public Nullable<DateTime> leave_date{ get; set; }
		/// <summary>
		/// 培训时间
		/// </summary>
		public Nullable<DateTime> training_date{ get; set; }
		/// <summary>
		/// 期数
		/// </summary>
		public string term{ get; set; }
		/// <summary>
		/// 主播职务
		/// </summary>
		public string position{ get; set; }
		/// <summary>
		/// 主播分类(根据主播的能力职务划分的等级A=厅管、副管、师傅、组长、A类主播；B级=B类主播；C=C类主播)
		/// </summary>
		public string level{ get; set; }
		/// <summary>
		/// 主播分级(萌新端在主播入职时划分的等级)
		/// </summary>
		public string zb_level{ get; set; }
		/// <summary>
		/// 说明
		/// </summary>
		public string note{ get; set; }
		/// <summary>
		/// 主播质量
		/// </summary>
		public Nullable<sbyte> quality{ get; set; }
		/// <summary>
		/// 分配优先级
		/// </summary>
		public Nullable<int> supplement_sort{ get; set; }
		/// <summary>
		/// 对接群 1
		/// </summary>
		public string qun{ get; set; }
		/// <summary>
		/// 流失原因（萌新操作）
		/// </summary>
		public string no_share{ get; set; }
		/// <summary>
		/// (待删除)数据来源#enum:外宣补人=1;转介绍=2;运营开通=3
		/// </summary>
		public Nullable<sbyte> data_sources{ get; set; }
		public enum data_sources_enum {
			外宣补人=1,
			转介绍=2,
			运营开通=3,
		}
		/// <summary>
		/// 来源名称
		/// </summary>
		public string sources_name{ get; set; }
		/// <summary>
		/// 来源备注
		/// </summary>
		public string sources_memo{ get; set; }
		/// <summary>
		/// 状态#enum:待开账号=0;正常=1;已离职=2;逻辑删除=9
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		public enum status_enum {
			待开账号=0,
			正常=1,
			已离职=2,
			逻辑删除=9,
		}
			}
	    			/// <summary>
			/// 表实体-用户信息-主播-日志 
			/// </summary>	
			public class user_info_zhubo_log : ModelDbBase
			{    
							public user_info_zhubo_log(){}
				public user_info_zhubo_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 操作类型#enum:新的主播=0;入职=1;离职=2;转厅=3;召回=4
		/// </summary>
		public Nullable<sbyte> c_type{ get; set; }
		public enum c_type_enum {
			新的主播=0,
			入职=1,
			离职=2,
			转厅=3,
			召回=4,
		}
		/// <summary>
		/// 关联user_info_zhubo的user_info_zb_sn
		/// </summary>
		public string user_info_zb_sn{ get; set; }
		/// <summary>
		/// 操作时主播状态#enum:待开账号=0;正常=1;已离职=2
		/// </summary>
		public Nullable<sbyte> zb_status{ get; set; }
		public enum zb_status_enum {
			待开账号=0,
			正常=1,
			已离职=2,
		}
		/// <summary>
		/// 内容描述
		/// </summary>
		public string content{ get; set; }
		/// <summary>
		/// 操作人用户类型id
		/// </summary>
		public Nullable<int> user_type_id{ get; set; }
		/// <summary>
		/// 操作人
		/// </summary>
		public string user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-用户-关系 
			/// </summary>	
			public class user_relation : ModelDbModularUserBasic.user_relation
			{    
							public user_relation(){}
				public user_relation(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.relation_type_id =  1;
														this.f_user_type_id =  1;
														this.t_user_type_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-用户-关系__组织id与基地sn 
			/// </summary>	
			public class user_relation__organize : ModelDbBase
			{    
							public user_relation__organize(){}
				public user_relation__organize(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.organize_id =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属组织部门id
		/// </summary>
		public Nullable<int> organize_id{ get; set; }
		/// <summary>
		/// 对应基地user_sn
		/// </summary>
		public string jd_user_sn{ get; set; }
			}
	    			/// <summary>
			/// 表实体-用户-关系-日志 
			/// </summary>	
			public class user_relation_log : ModelDbModularUserBasic.user_relation_log
			{    
							public user_relation_log(){}
				public user_relation_log(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.user_type_id =  1;
														this.relation_type_id =  1;
														this.f_user_type_id =  1;
														this.t_user_type_id =  1;
												}
				}
			public enum o_type_enum {
			转移用户=1,
			绑定用户=2,
			解绑用户=3,
		}
			}
	    			/// <summary>
			/// 表实体-用户-关系类型 
			/// </summary>	
			public class user_relation_type : ModelDbModularUserBasic.user_relation_type
			{    
							public user_relation_type(){}
				public user_relation_type(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.f_user_type_id =  1;
														this.t_user_type_id =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-用户-角色明细 
			/// </summary>	
			public class user_role : ModelDbModularBasic.user_role
			{    
							public user_role(){}
				public user_role(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.position_id =  1;
														this.role_id =  1;
												}
				}
			public enum is_default_enum {
			否=0,
			是=1,
		}
			}
	    			/// <summary>
			/// 表实体-用户-类型 
			/// </summary>	
			public class user_type : ModelDbModularBasic.user_type
			{    
							public user_type(){}
				public user_type(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.sort =  1;
												}
				}
						}
	    			/// <summary>
			/// 表实体-学习中心-文章 
			/// </summary>	
			public class xuexi_base : ModelDbBase
			{    
							public xuexi_base(){}
				public xuexi_base(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.category_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 所属类别id
		/// </summary>
		public Nullable<int> category_id{ get; set; }
		/// <summary>
		/// 标题
		/// </summary>
		public string title{ get; set; }
		/// <summary>
		/// 简要介绍
		/// </summary>
		public string introduction{ get; set; }
		/// <summary>
		/// 链接地址
		/// </summary>
		public string url{ get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		/// <summary>
		/// 排序号
		/// </summary>
		public Nullable<int> sort{ get; set; }
			}
	    			/// <summary>
			/// 表实体-学习中心-类别 
			/// </summary>	
			public class xuexi_category : ModelDbBase
			{    
							public xuexi_category(){}
				public xuexi_category(bool isDefault = false)
				{
					if (isDefault)
					{
													this.id =  1;
														this.tenant_id =  1;
														this.sort =  1;
												}
				}
			/// <summary>
		/// 租户id
		/// </summary>
		public Nullable<int> tenant_id{ get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string name{ get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public Nullable<sbyte> status{ get; set; }
		/// <summary>
		/// 排序号
		/// </summary>
		public Nullable<int> sort{ get; set; }
			}
	    	}
}
 
