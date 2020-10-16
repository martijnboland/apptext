import React from 'react';
import { useTranslation } from 'react-i18next';
import ReactMarkdown from 'react-markdown';
import Loader from './loader/Loader';
import { useAppTextPage } from './localization/useAppTextPage';

interface IntroProps {
  onCreateNote(): void
}

const Intro: React.FC<IntroProps> = ({ onCreateNote }) => {
  const { t, i18n } = useTranslation(['labels','messages']);
  const { page, fetching, error } = useAppTextPage('intro', i18n.language);

  return (
    <section className="intro">
      {page
        ? 
        <React.Fragment>
          <h2>{page.title}</h2>
          <ReactMarkdown source={page.content} />
          <button onClick={onCreateNote}>{t('labels:Create note')}</button>
        </React.Fragment>
        : fetching
          ? <Loader />
          : <p>{t('messages:Page not found', { contentKey: 'intro' })}</p>
      }
      {error &&
        <p>{error.message}</p>
      }
    </section>
  )
}

export default Intro;