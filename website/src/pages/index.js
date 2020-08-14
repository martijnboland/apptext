import React from 'react';
import clsx from 'clsx';
import Layout from '@theme/Layout';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import useBaseUrl from '@docusaurus/useBaseUrl';
import styles from './styles.module.css';

const features = [
  {
    imageUrl: 'img/undraw_relaxation_1_wbr7.svg',
    title: <>No deployment</>,
    description: (
      <>
        Update content such as labels, tooltips or help pages in your application
        without having to redeploy new versions of your application.
      </>
    ),
  },
  {
    imageUrl: 'img/undraw_Around_the_world_re_n353.svg',
    title: <>Multi-language</>,
    description: (
      <>
        Edit content in multiple languages side-by-side and add new languages on the fly.
      </>
    ),
  },
  {
    imageUrl: 'img/undraw_server_cluster_jwwq.svg',
    title: <>Powered by ASP.NET Core</>,
    description: (
      <>
        AppText runs as add-on in <a href="https://dotnet.microsoft.com/apps/aspnet">ASP.NET Core</a> host 
        applications and you <a href="docs/installation">install it</a> from
        <a href="https://www.nuget.org/packages/AppText/">NuGet packages</a>.
      </>
    ),
  },
];

function Feature({imageUrl, title, description}) {
  const imgUrl = useBaseUrl(imageUrl);
  return (
    <div className={clsx('col col--4', styles.feature)}>
      {imgUrl && (
        <div className="text--center">
          <img className={styles.featureImage} src={imgUrl} alt={title} />
        </div>
      )}
      <h3>{title}</h3>
      <p>{description}</p>
    </div>
  );
}

function Home() {
  const context = useDocusaurusContext();
  const {siteConfig = {}} = context;
  return (
    <Layout
      title={`${siteConfig.title} | ${siteConfig.tagline}`}
      description="Description will go into a meta tag in <head />">
      <header className={clsx('hero hero--primary', styles.heroBanner)}>
        <div className="container">
          <h1 className="hero__title">{siteConfig.title}</h1>
          <p className="hero__subtitle">{siteConfig.tagline}</p>
          <div className={styles.buttons}>
            <Link
              className={clsx(
                'button button--secondary button--lg',
                styles.getStarted,
              )}
              to={useBaseUrl('docs/installation')}>
              Get Started
            </Link>
            <Link
              className={clsx(
                'button button--secondary button--lg',
                styles.getStarted,
              )}
              to={useBaseUrl('docs/')}>
              Learn more
            </Link>
          </div>
        </div>
      </header>
      <main>
        {features && features.length > 0 && (
          <section className={styles.features}>
            <div className="container">
              <div className="row">
                {features.map((props, idx) => (
                  <Feature key={idx} {...props} />
                ))}
              </div>
            </div>
          </section>
        )}
      </main>
    </Layout>
  );
}

export default Home;
