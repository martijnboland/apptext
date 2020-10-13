import React from 'react';

interface IntroProps {
  onCreateNote(): void
}

const Intro: React.FC<IntroProps> = ({ onCreateNote }) => {
  return (
    <section className="intro">
      <h2>Intro</h2>
      <button onClick={onCreateNote}>Create note</button>
    </section>
  )
}

export default Intro;