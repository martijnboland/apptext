import React, { Suspense, useRef, useState } from 'react';
import { CSSTransition } from 'react-transition-group';
import Header from './Header';
import Intro from './Intro';
import CreateNote from './notes/CreateNote';
import ListNotes from './notes/ListNotes';

import './localization/i18n';
import './App.css';
import { Note } from './notes/models';

function App() {
  const createFormRef = useRef(null);
  const [ showIntro, setShowIntro ] = useState(true);
  const [ showForm, setShowForm ] = useState(false);
  const [ notes, setNotes ] = useState<Note[]>(
    JSON.parse(localStorage.getItem('NOTES') || '[]') || []
  );

  React.useEffect(() => {
    localStorage.setItem('NOTES', JSON.stringify(notes));
  }, [notes]);

  const addNote = (note: Note): Promise<void> => {
    setNotes([...notes, note]);
    return Promise.resolve();
  }

  const removeNote = (note: Note): Promise<void> => {
    const newNotes = notes.filter(n => n !== note);
    setNotes(newNotes);
    return Promise.resolve();
  }

  return (
    <Suspense fallback="loading">
      <div className="app">
        <Header />
        {showIntro &&
          <Intro onCreateNote={() => setShowForm(true)} />
        }
        <CSSTransition
          nodeRef={createFormRef}
          in={showForm}
          timeout={300}
          classNames="notes-form"
          unmountOnExit
          onEnter={() => setShowIntro(false)}
          onExited={() => setShowIntro(true)}
        >
          <CreateNote ref={createFormRef} onClose={() => setShowForm(false)} onCreate={addNote} />
        </CSSTransition>
        <ListNotes notes={notes} onRemoveNote={removeNote} />
      </div>
    </Suspense>
  );
}

export default App;
