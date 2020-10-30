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
    title: <>Easy installation</>,
    description: (
      <>
        Install AppText as <a href="/docs/installation#install-with-docker">Docker container</a> or 
        embed it as <a href="/docs/installation#install-into-an-aspnet-core-host-application">add-on in ASP.NET Core host applications</a>.
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
      title={`${siteConfig.tagline}`}
      description="AppText is an ASP.NET Core Content Management System for Applications. A hybrid between a headless Content Management System and a Translation Management System.">
      <header className={clsx('hero hero--primary', styles.heroBanner)}>
        <div className="container">
          <div className="row">
            <div className="col">
              <h1 className="hero__title">{siteConfig.title}</h1>
              <p className="hero__subtitle">{siteConfig.tagline}</p>
            </div>
            <div className={clsx('col')}>
              <p>
                Custom applications often ship with embedded content. Think of labels, tooltips or even complete pages with information.
              </p>
              <p>
                Once an application is released, updating the embedded content can become a bit of burden. Even the smallest textual change often requires building and deploying a new version.
              </p>
              <p>
                AppText enables content updates in applications without having to go through the entire process of deploying a new version of the application.
              </p>
              <div className={styles.buttons}>
                <Link
                  className={clsx(
                    'button button--secondary button--lg',
                    styles.getStarted,
                  )}
                  to={useBaseUrl('docs/live-demo')}>
                  Live demo
                </Link>
              </div>
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
