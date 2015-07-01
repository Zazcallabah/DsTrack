# DsTrack

My theory is as follows.

Some things are most likely stored in the save file as flags, such as lighting bonfires or opening doors etc.

Given a series of snapshots of the same save slot at different points in time, with labels clearly marking
what they contain (like 'lit firelink bonfire' or 'opened undead burg shortcut'), we should be able to see
which actions map to which bits in the save file.

We only consider bits that start at 0 and go to 1 and never ever regress. (You can't reclose a door when it has been opened etc.)