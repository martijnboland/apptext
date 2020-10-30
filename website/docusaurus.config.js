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
        src: 'img/apptext-logo-64x64.png',
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
      copyright: `Copyright © ${new Date().getFullYear()} AppText. Powered by Docusaurus. Enabled by <a href="https://taiga.nl/en">Taiga</a>.`,
    },
    prism: {
      additionalLanguages: ['csharp'],
    }
  },
  presets: [
    [
      '@docusaurus/preset-classic',
      {
        docs: {
          path: 'docs',
          
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
