export default {
  /**
   * 框架版本号
   */
  version: '3.8.2',
  /**
   * 网页标题
   */
  title: import.meta.env.VITE_APP_TITLE,
  /**
   * 侧边栏主题 深色主题theme-dark，浅色主题theme-light
   */
  sideTheme: 'theme-dark',
  /**
   * 框架主题颜色值
   */
  theme: '#109dea',
  /**
   * 是否系统布局配置
   */
  showSettings: false,

  /**
   * 是否显示顶部导航
   */
  topNav: false,

  /**
   * 是否显示 tagsView
   */
  tagsView: true,

  /**
   * 是否固定头部
   */
  fixedHeader: false,

  /**
   * 是否显示logo
   */
  sidebarLogo: true,

  /**
   * 是否显示动态标题
   */
  dynamicTitle: false,

  /**
   * @type {string | array} 'production' | ['production', 'development']
   * @description Need show err logs component.
   * The default is only used in the production env
   * If you want to also use it in dev, you can pass ['production', 'development']
   */
  errorLog: 'production',
  /**
   * 版权信息
   */
  copyright: '',
  /**
   * 是否显示底部栏
   */
  showFooter: true,
  /**
   * 是否显示水印
   */
  showWatermark: false,
  /**
   * 水印文案
   */
  watermarkText: 'ARWAdmin.NET'
}