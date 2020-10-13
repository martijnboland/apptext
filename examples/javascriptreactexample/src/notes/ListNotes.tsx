import React from 'react';
import { Note } from './models';

interface ListProps {
  notes: Note[],
  onRemoveNote(note: Note): void 
}

const List: React.FC<ListProps> = ({ notes, onRemoveNote }) => {
  return (
    <section className="notes">
      <h2>Notes</h2>
      <p>There are {notes.length} notes</p>
      {notes.map((note, idx) => 
        <div key={idx}>
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