import React from 'react';
import { useTranslation } from 'react-i18next';
import ReactMarkdown from 'react-markdown';
import { useAppTextPage } from './localization/useAppTextPage';

interface IntroProps {
  onCreateNote(): void
}

const Intro: React.FC<IntroProps> = ({ onCreateNote }) => {
  const { t, i18n } = useTranslation('labels');
  const { page } = useAppTextPage('intro', i18n.language);

  return (
    <section className="intro">
      {page && 
        <React.Fragment>
          <h2>{page.title}</h2>
          <ReactMarkdown source={page.content} />
          <button onClick={onCreateNote}>{t('Create note')}</button>
        </React.Fragment>
      }
    </section>
  )
}

export default Intro;