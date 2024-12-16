// Create 50 Ports
CREATE (p1:Port {name: 'Rotterdam'}),
       (p2:Port {name: 'Hamburg'}),
       (p3:Port {name: 'Copenhagen'}),
       (p4:Port {name: 'Oslo'}),
       (p5:Port {name: 'Amsterdam'}),
       (p6:Port {name: 'Antwerp'}),
       (p7:Port {name: 'Stockholm'}),
       (p8:Port {name: 'Le Havre'}),
       (p9:Port {name: 'Bremen'}),
       (p10:Port {name: 'Marseille'}),
       (p11:Port {name: 'Valencia'}),
       (p12:Port {name: 'Gothenburg'}),
       (p13:Port {name: 'Naples'}),
       (p14:Port {name: 'Trieste'}),
       (p15:Port {name: 'Palma'}),
       (p16:Port {name: 'Lisbon'}),
       (p17:Port {name: 'Bergen'}),
       (p18:Port {name: 'Barcelona'}),
       (p19:Port {name: 'Malmö'}),
       (p20:Port {name: 'Turku'}),
       (p21:Port {name: 'Stavanger'}),
       (p22:Port {name: 'Aarhus'}),
       (p23:Port {name: 'Helsinki'}),
       (p24:Port {name: 'Bilbao'}),
       (p25:Port {name: 'Genoa'}),
       (p26:Port {name: 'Tallinn'}),
       (p27:Port {name: 'Riga'}),
       (p28:Port {name: 'Klaipeda'}),
       (p29:Port {name: 'St. Petersburg'}),
       (p30:Port {name: 'Kiel'}),
       (p31:Port {name: 'Bruges'}),
       (p32:Port {name: 'Fredrikstad'}),
       (p33:Port {name: 'Reykjavik'}),
       (p34:Port {name: 'Dublin'}),
       (p35:Port {name: 'Belfast'}),
       (p36:Port {name: 'Cork'}),
       (p37:Port {name: 'Portsmouth'}),
       (p38:Port {name: 'Southampton'}),
       (p39:Port {name: 'London'}),
       (p40:Port {name: 'Liverpool'}),
       (p41:Port {name: 'Glasgow'}),
       (p42:Port {name: 'Newcastle'}),
       (p43:Port {name: 'Aberdeen'}),
       (p44:Port {name: 'Cardiff'}),
       (p45:Port {name: 'Zeebrugge'}),
       (p46:Port {name: 'Gdansk'}),
       (p47:Port {name: 'Split'}),
       (p48:Port {name: 'Venice'}),
       (p49:Port {name: 'Constanta'}),
       (p50:Port {name: 'Bourgas'});

// Create Docking Relationships for Vessels stored in Neo4j

// Docking at Rotterdam
MATCH (p1:Port {name: 'Rotterdam'})
CREATE (p1)<-[:DOCKS_AT {vessel_name: 'Poseidon', arrival_date: date('2024-10-08'), departure_date: date('2024-10-12')}]-(),
       (p1)<-[:DOCKS_AT {vessel_name: 'Mariner', arrival_date: date('2024-10-18'), departure_date: date('2024-10-23')}]-(),
       (p1)<-[:DOCKS_AT {vessel_name: 'Horizon', arrival_date: date('2024-11-01'), departure_date: date('2024-11-05')}]-(),
       (p1)<-[:DOCKS_AT {vessel_name: 'Voyager', arrival_date: date('2024-11-10'), departure_date: date('2024-11-14')}]-();

// Docking at Hamburg
MATCH (p2:Port {name: 'Hamburg'})
CREATE (p2)<-[:DOCKS_AT {vessel_name: 'Titanic II', arrival_date: date('2024-10-06'), departure_date: date('2024-10-09')}]-(),
       (p2)<-[:DOCKS_AT {vessel_name: 'Enterprise', arrival_date: date('2024-10-20'), departure_date: date('2024-10-25')}]-(),
       (p2)<-[:DOCKS_AT {vessel_name: 'Catalyst', arrival_date: date('2024-11-03'), departure_date: date('2024-11-08')}]-(),
       (p2)<-[:DOCKS_AT {vessel_name: 'Discovery', arrival_date: date('2024-11-15'), departure_date: date('2024-11-19')}]-();

// Docking at Copenhagen
MATCH (p3:Port {name: 'Copenhagen'})
CREATE (p3)<-[:DOCKS_AT {vessel_name: 'Neptune', arrival_date: date('2024-11-01'), departure_date: date('2024-11-05')}]-(),
       (p3)<-[:DOCKS_AT {vessel_name: 'Liberty', arrival_date: date('2024-10-15'), departure_date: date('2024-10-20')}]-(),
       (p3)<-[:DOCKS_AT {vessel_name: 'Explorer II', arrival_date: date('2024-11-10'), departure_date: date('2024-11-15')}]-(),
       (p3)<-[:DOCKS_AT {vessel_name: 'Galileo', arrival_date: date('2024-11-20'), departure_date: date('2024-11-25')}]-();

// Docking at Oslo
MATCH (p4:Port {name: 'Oslo'})
CREATE (p4)<-[:DOCKS_AT {vessel_name: 'Endeavour', arrival_date: date('2024-10-15'), departure_date: date('2024-10-20')}]-(),
       (p4)<-[:DOCKS_AT {vessel_name: 'Pioneer', arrival_date: date('2024-11-05'), departure_date: date('2024-11-09')}]-(),
       (p4)<-[:DOCKS_AT {vessel_name: 'Odyssey', arrival_date: date('2024-11-18'), departure_date: date('2024-11-22')}]-(),
       (p4)<-[:DOCKS_AT {vessel_name: 'Voyager', arrival_date: date('2024-12-01'), departure_date: date('2024-12-05')}]-();

// Docking at Antwerp
MATCH (p6:Port {name: 'Antwerp'})
CREATE (p6)<-[:DOCKS_AT {vessel_name: 'Discovery', arrival_date: date('2024-11-02'), departure_date: date('2024-11-06')}]-(),
       (p6)<-[:DOCKS_AT {vessel_name: 'Neptune', arrival_date: date('2024-10-25'), departure_date: date('2024-10-30')}]-(),
       (p6)<-[:DOCKS_AT {vessel_name: 'Poseidon', arrival_date: date('2024-11-11'), departure_date: date('2024-11-16')}]-(),
       (p6)<-[:DOCKS_AT {vessel_name: 'Enterprise', arrival_date: date('2024-12-05'), departure_date: date('2024-12-10')}]-();

// Docking at Stockholm
MATCH (p7:Port {name: 'Stockholm'})
CREATE (p7)<-[:DOCKS_AT {vessel_name: 'Liberty', arrival_date: date('2024-10-09'), departure_date: date('2024-10-13')}]-(),
       (p7)<-[:DOCKS_AT {vessel_name: 'Horizon', arrival_date: date('2024-11-10'), departure_date: date('2024-11-15')}]-(),
       (p7)<-[:DOCKS_AT {vessel_name: 'Mariner', arrival_date: date('2024-11-20'), departure_date: date('2024-11-25')}]-(),
       (p7)<-[:DOCKS_AT {vessel_name: 'Odyssey', arrival_date: date('2024-12-05'), departure_date: date('2024-12-10')}]-();

// Docking at Amsterdam
MATCH (p5:Port {name: 'Amsterdam'})
CREATE (p5)<-[:DOCKS_AT {vessel_name: 'Odyssey', arrival_date: date('2024-10-20'), departure_date: date('2024-10-24')}]-(),
       (p5)<-[:DOCKS_AT {vessel_name: 'Voyager', arrival_date: date('2024-11-15'), departure_date: date('2024-11-20')}]-(),
       (p5)<-[:DOCKS_AT {vessel_name: 'Explorer II', arrival_date: date('2024-11-25'), departure_date: date('2024-11-30')}]-(),
       (p5)<-[:DOCKS_AT {vessel_name: 'Galileo', arrival_date: date('2024-12-01'), departure_date: date('2024-12-06')}]-();

// Docking at Marseille
MATCH (p10:Port {name: 'Marseille'})
CREATE (p10)<-[:DOCKS_AT {vessel_name: 'Enterprise', arrival_date: date('2024-11-10'), departure_date: date('2024-11-15')}]-(),
       (p10)<-[:DOCKS_AT {vessel_name: 'Pioneer', arrival_date: date('2024-12-05'), departure_date: date('2024-12-10')}]-(),
       (p10)<-[:DOCKS_AT {vessel_name: 'Catalyst', arrival_date: date('2024-12-11'), departure_date: date('2024-12-16')}]-(),
       (p10)<-[:DOCKS_AT {vessel_name: 'Discovery', arrival_date: date('2024-12-20'), departure_date: date('2024-12-24')}]-();
