1.登陆时设置系统语言（系统语言有公司维度、员工维度）
在Web展现层下WebApplication.cs下设置
2.根据权限隐藏左侧导航
在总的Module下Controller/HideNavigationController.cs中设置
3.根据权限隐藏按钮：如器具管理
在Module.Web/HideActionWebController.cs