import React from 'react';
import { useTranslation } from 'react-i18next';

interface IntroProps {
  onCreateNote(): void
}

const Intro: React.FC<IntroProps> = ({ onCreateNote }) => {
  const { t } = useTranslation('labels');

  return (
    <section className="intro">
      <h2>{t('Intro')}</h2>
      <button onClick={onCreateNote}>{t('Create note')}</button>
    </section>
  )
}

export default Intro;