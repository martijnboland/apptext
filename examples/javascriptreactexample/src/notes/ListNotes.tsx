import React from 'react';
import { useTranslation } from 'react-i18next';
import { Note } from './models';

interface ListProps {
  notes: Note[],
  onRemoveNote(note: Note): void 
}

const List: React.FC<ListProps> = ({ notes, onRemoveNote }) => {
  const { t } = useTranslation('labels');

  return (
    <section className="notes">
      <h2>{t('Notes')}</h2>
      <p>{t('There are n notes', { count: notes.length })}</p>
      {notes.map((note, idx) => 
        <div key={idx} className="card">
          <div className="heading-withbutton">
            <h3>{note.title}</h3>
            <div>
              <button onClick={() => onRemoveNote(note)}>&times;</button>
            </div>
          </div>
          <p>{note.content}</p>
        </div>
      )}
    </section>
  )
}

export default List;