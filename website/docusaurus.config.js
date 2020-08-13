module.exports = {
  title: 'AppText',
  tagline: 'Content Management for Applications',
  url: 'https://apptext.io',
  baseUrl: '/',
  onBrokenLinks: 'throw',
  favicon: 'img/favicon.ico',
  organizationName: 'martijnboland', // Usually your GitHub org/user name.
  projectName: 'apptext', // Usually your repo name.
  themeConfig: {
    navbar: {
      title: 'AppText',
      logo: {
        alt: 'AppText Logo',
        src: 'img/logo.svg',
      },
      items: [
        {
          to: 'docs/',
          activeBasePath: 'docs',
          label: 'Docs',
          position: 'left',
        },
        {
          href: 'https://github.com/martijnboland/apptext',
          label: 'GitHub',
          position: 'right',
        },
      ],
    },
    footer: {
      style: 'dark',
      links: [
      ],
      copyright: `Copyright © ${new Date().getFullYear()} AppText. Built with Docusaurus.`,
    },
  },
  presets: [
    [
      '@docusaurus/preset-classic',
      {
        docs: {
          path: 'docs',
          
          // It is recommended to set document id as docs home page (`docs/` path).
          homePageId: 'introduction',
          sidebarPath: require.resolve('./sidebars.js'),
          // Please change this to your repo.
          editUrl:
            'https://github.com/martijnboland/apptext/edit/main/website',
        },
        theme: {
          customCss: require.resolve('./src/css/custom.css'),
        },
      },
    ],
  ],
};
